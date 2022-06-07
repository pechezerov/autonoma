using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Configuration.Views
{
    public partial class ProjectView : UserControl
    {
        public ProjectView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
