using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Presentation.Controls
{
    public partial class PropertiesEditor : UserControl
    {
        public PropertiesEditor()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
