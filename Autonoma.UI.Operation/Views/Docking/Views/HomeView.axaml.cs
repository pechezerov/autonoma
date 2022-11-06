using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Operation.Views.Docking.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}