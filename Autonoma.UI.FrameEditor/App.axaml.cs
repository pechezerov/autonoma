using Autonoma.UI.FrameEditor.Models;
using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.FrameEditor.Views;
using Autonoma.UI.Presentation;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Infrastructure;
using Autonoma.UI.Presentation.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Dock.Model.Core;
using ReactiveUI;

namespace Autonoma.UI.FrameEditor
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var mainWindowViewModel = new MainWindowViewModel();
            var layout = mainWindowViewModel.Layout;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var mainWindow = new MainWindow
                {
                    DataContext = mainWindowViewModel
                };

                mainWindow.Closing += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };

                desktopLifetime.MainWindow = mainWindow;

                desktopLifetime.Exit += (_, _) =>
                {
                    if (layout is IDock dock)
                    {
                        if (dock.Close.CanExecute(null))
                        {
                            dock.Close.Execute(null);
                        }
                    }
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
