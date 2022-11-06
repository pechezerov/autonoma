using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Operation.Views.Docking.Views;

public partial class HomeStatusBarView : UserControl
{
    public HomeStatusBarView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}