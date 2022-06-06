using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.FrameEditor.Views.Tools
{
    public partial class ToolboxToolView : UserControl
    {
        public ToolboxToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
