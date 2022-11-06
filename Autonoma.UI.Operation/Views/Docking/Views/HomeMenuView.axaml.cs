using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Operation.Views.Docking.Views;

public partial class HomeMenuView : UserControl
{
    public HomeMenuView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}