using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.FrameEditor.Views
{
    public partial class ConnectionManager : UserControl
    {
        public ConnectionManager()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
