using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;
using Core2D.ViewModels.Shapes;
using Core2D.ViewModels.Style;

namespace Autonoma.UI.Shapes.Electric
{
    public interface IAutonomaViewModelFactory
    {
        ElectricLineShapeViewModel CreateElectricLineShape(PointShapeViewModel? start, PointShapeViewModel? end, ShapeStyleViewModel? style, bool isStroked = true, string name = "");

        ElectricLineShapeViewModel CreateElectricLineShape(double x1, double y1, double x2, double y2, ShapeStyleViewModel? style, bool isStroked = true, string name = "");

        ElectricLineShapeViewModel CreateElectricLineShape(double x, double y, ShapeStyleViewModel? style, bool isStroked = true, string name = "");


        ElectricSwitchShapeViewModel CreateElectricSwitchShape(double x1, double y1, double x2, double y2, ShapeStyleViewModel? style, string name = "");

        ElectricSwitchShapeViewModel CreateElectricSwitchShape(double x, double y, ShapeStyleViewModel? style, string name = "");

        ElectricSwitchShapeViewModel CreateElectricSwitchShape(PointShapeViewModel? topLeft, PointShapeViewModel? bottomRight, ShapeStyleViewModel? style, string name = "");

    }
}
