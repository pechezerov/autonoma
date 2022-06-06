using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.FrameEditor.Views.Tools
{
    public partial class FrameMonitorToolView : UserControl
    {
        public FrameMonitorToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
