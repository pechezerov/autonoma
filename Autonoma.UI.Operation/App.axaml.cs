using Autonoma.UI.Operation.ViewModels;
using Autonoma.UI.Operation.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Operation
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
                var communicationService = new CommunicationService();
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(communicationService),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
