using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Shapes.Electric.Views.Shapes;

public class ElectricSwitchShapeView : UserControl
{
    public ElectricSwitchShapeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
