using Autonoma.UI.Operation.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia.Headless;
using Avalonia.OpenGL;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Core2D.Util;
using Core2D.ViewModels.Editor;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.UI.Operation
{
    internal class Program
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool AttachConsole(int processId);

        private static Thread? s_replThread;

        private static void Log(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException is { })
            {
                Log(ex.InnerException);
            }
        }

        [STAThread]
        public static void Main(string[] args)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                AttachConsole(-1);

            var rootCommand = CreateRootCommand();
            var rootSettings = default(Settings?);

            rootCommand.Handler = CommandHandler.Create((Settings settings) =>
            {
                rootSettings = settings;
            });

            rootCommand.Invoke(args);

            if (rootSettings is { })
                StartAvaloniaApp(rootSettings, args);
        }

        private static RootCommand CreateRootCommand()
        {
            var rootCommand = new RootCommand()
            {
                Description = "A multi-platform data driven 2D diagram editor."
            };

            rootCommand.AddOption(new Option(new[] { "--theme", "-t" }, "Set application theme", typeof(string)));
            rootCommand.AddOption(new Option(new[] { "--scripts", "-s" }, "The relative or absolute path to the script files", typeof(FileInfo[])));
            rootCommand.AddOption(new Option(new[] { "--project", "-p" }, "The relative or absolute path to the project file", typeof(FileInfo)));
            rootCommand.AddOption(new Option(new[] { "--repl" }, "Run scripting repl", typeof(bool)));
            rootCommand.AddOption(new Option(new[] { "--useManagedSystemDialogs" }, "Use managed system dialogs", typeof(bool)));
#if ENABLE_DIRECT2D1
        rootCommand.AddOption(new Option(new[] { "--useDirect2D1" }, "Use Direct2D1 renderer", typeof(bool)));
#endif
            rootCommand.AddOption(new Option(new[] { "--useSkia" }, "Use Skia renderer", typeof(bool)));
            rootCommand.AddOption(new Option(new[] { "--enableMultiTouch" }, "Enable multi-touch", typeof(bool), () => true));
            rootCommand.AddOption(new Option(new[] { "--useGpu" }, "Use Gpu", typeof(bool), () => true));
            rootCommand.AddOption(new Option(new[] { "--allowEglInitialization" }, "Allow EGL initialization", typeof(bool), () => true));
            rootCommand.AddOption(new Option(new[] { "--useWgl" }, "Use Windows GL", typeof(bool)));
            rootCommand.AddOption(new Option(new[] { "--useDeferredRendering" }, "Use deferred rendering", typeof(bool), () => true));
            rootCommand.AddOption(new Option(new[] { "--useWindowsUIComposition" }, "Use Windows UI composition", typeof(bool), () => true));
            rootCommand.AddOption(new Option(new[] { "--useDirectX11" }, "Use DirectX11 platform api", typeof(bool)));
            rootCommand.AddOption(new Option(new[] { "--useHeadless" }, "Use headless", typeof(bool)));
            rootCommand.AddOption(new Option(new[] { "--useHeadlessDrawing" }, "Use headless drawing", typeof(bool)));

            return rootCommand;
        }

        private static void StartAvaloniaApp(Settings settings, string[] args)
        {
            var builder = BuildAvaloniaApp();

            try
            {
                if (settings.Theme is { })
                {
                    App.DefaultTheme = settings.Theme;
                }

                if (settings.Repl)
                {
                    RunRepl();
                }

#if ENABLE_DIRECT2D1
            if (settings.UseDirect2D1)
            {
                builder.UseDirect2D1();
            }
#endif

                if (settings.UseSkia)
                {
                    builder.UseSkia();
                }

                builder.With(new X11PlatformOptions
                {
                    UseGpu = settings.UseGpu,
                    UseDeferredRendering = settings.UseDeferredRendering
                });

                builder.With(new Win32PlatformOptions
                {
                    AllowEglInitialization = settings.AllowEglInitialization,
                    UseWgl = settings.UseWgl,
                    UseDeferredRendering = settings.UseDeferredRendering,
#if ENABLE_DIRECT2D1
                UseWindowsUIComposition = !settings.UseDirect2D1 && settings.UseWindowsUIComposition
#else
                    UseWindowsUIComposition = settings.UseWindowsUIComposition
#endif
                });

                if (settings.UseDirectX11)
                {
                    builder.With(new AngleOptions()
                    {
                        AllowedPlatformApis = new List<AngleOptions.PlatformApi>
                    {
                        AngleOptions.PlatformApi.DirectX11
                    }
                    });
                }

                if (settings.UseManagedSystemDialogs)
                {
                    builder.UseManagedSystemDialogs();
                }

                if (settings.UseHeadless)
                {
                    builder.UseHeadless(new AvaloniaHeadlessPlatformOptions { UseCompositor = true, UseHeadlessDrawing = settings.UseHeadlessDrawing });
                }

                try
                {
                    builder.AfterSetup(async _ => await ProcessSettings(settings))
                        .StartWithClassicDesktopLifetime(args);
                }
                catch (Exception e)
                {
                    Log(e);
                }
                finally
                {
                }

            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }
        private static async Task ProcessSettings(Settings settings)
        {
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                var applicationLifetime = (IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
                var mainWindow = applicationLifetime?.MainWindow;
                var mainView = mainWindow?.FindControl<MainView>("mainView");
                var editor = mainView?.DataContext as ProjectEditorViewModel;

                if (mainView is { })
                {
                    if (settings.Scripts is { })
                    {
                        foreach (var script in settings.Scripts)
                        {
                            editor?.OnExecuteScriptFile(script.FullName);
                            Dispatcher.UIThread.RunJobs();
                        }
                    }

                    if (settings.Project is { })
                    {
                        editor?.OnOpenProject(settings.Project.FullName);
                        Dispatcher.UIThread.RunJobs();
                    }
                }
            });
        }

        private static void RunRepl()
        {
            s_replThread = new Thread(ReplThread)
            {
                IsBackground = true
            };
            s_replThread?.Start();
        }

        private static async void ReplThread()
        {
            ScriptState<object>? state = null;

            while (true)
            {
                try
                {
                    var code = Console.ReadLine();

                    if (state is { } previous)
                    {
                        await Utilities.RunUiJob(async () => { state = await previous.ContinueWithAsync(code); });
                    }
                    else
                    {
                        await Utilities.RunUiJob(async () =>
                        {
                            var options = ScriptOptions.Default.WithImports("System");
                            state = await CSharpScript.RunAsync(code, options, new Repl());
                        });
                    }
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToTrace();
    }
}
