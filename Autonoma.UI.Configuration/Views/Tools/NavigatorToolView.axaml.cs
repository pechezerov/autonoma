using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Configuration.Views.Tools
{
    public partial class NavigatorToolView : UserControl
    {
        public NavigatorToolView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
