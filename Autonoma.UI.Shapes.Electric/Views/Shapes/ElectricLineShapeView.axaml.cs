using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Shapes.Electric.Views.Shapes;

public class ElectricLineShapeView : UserControl
{
    public ElectricLineShapeView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
