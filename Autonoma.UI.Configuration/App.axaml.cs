using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using Autonoma.UI.Configuration.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Configuration
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(new MainDockFactory(), new ProjectSerializer()),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
