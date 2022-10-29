using Autonoma.Configuration;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using Autonoma.UI.Configuration.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace Autonoma.UI.Configuration
{
    public partial class App : Application
    {
        private IHost _host = null!;
        private IServiceScope _serviceScope = null!;
        private IServiceProvider _provider = null!;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var appConfiguration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                _host = Host.CreateDefaultBuilder()
                   .ConfigureServices((_, services) =>
                   {
                        // configuration file
                       services.AddSingleton<IConfiguration>(appConfiguration)
                               .AddCustomDbContext(appConfiguration, LoggerFactory.Create(builder => { builder.AddConsole(); }));

                       services.AddTransient<IProjectSerializer, SqliteProjectSerializer>();
                   })
                   .Build();

                _serviceScope = _host.Services.CreateScope();
                _provider = _serviceScope.ServiceProvider;

                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new MainDockFactory(), _provider),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
