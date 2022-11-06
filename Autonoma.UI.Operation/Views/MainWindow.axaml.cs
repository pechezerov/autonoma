using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Core2D.Screenshot;

namespace Autonoma.UI.Operation.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            this.AttachCapture();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
        }
    }
}
