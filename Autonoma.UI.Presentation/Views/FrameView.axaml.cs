using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Presentation.Views
{
    public partial class FrameView : UserControl
    {
        public FrameView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
