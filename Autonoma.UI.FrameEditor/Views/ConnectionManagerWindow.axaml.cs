using Autonoma.UI.FrameEditor.Views.Tools;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Autonoma.UI.FrameEditor.Views
{
    public partial class ConnectionManagerWindow : ReactiveWindow<ConnectionManagerViewModel>
    {
        public ConnectionManagerWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
