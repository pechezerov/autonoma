using Autonoma.UI.FrameEditor.ViewModels;
using Autonoma.UI.Presentation.Model;
using Autonoma.UI.Presentation.ViewModels;
using Autonoma.UI.Presentation.Views;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;

namespace Autonoma.UI.FrameEditor.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowConnectionManagerDialog.RegisterHandler(DoShowConnectionManagerDialogAsync)));
        }

        private async Task DoShowConnectionManagerDialogAsync(InteractionContext<ConnectionManagerViewModel, DataPointConnection?> interaction)
        {
            var dialog = new ConnectionManagerWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<DataPointConnection?>(this);
            interaction.SetOutput(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
