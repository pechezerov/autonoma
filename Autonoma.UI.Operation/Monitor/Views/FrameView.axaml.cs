using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Operation.Monitor.Views
{
    public partial class FrameView : UserControl
    {
        public FrameView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
