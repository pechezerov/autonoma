using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;
using Core2D.Model;
using Core2D.Model.Renderer;
using Core2D.ViewModels.Data;
using Core2D.ViewModels.Shapes;
using Core2D.ViewModels.Style;
using System.Collections.Immutable;

namespace Autonoma.UI.Shapes.Electric
{
    public class AutonomaViewModelFactory : IAutonomaViewModelFactory
    {
        private readonly IServiceProvider? _serviceProvider;
        private readonly IViewModelFactory _baseModelFactory;

        public AutonomaViewModelFactory(IServiceProvider? serviceProvider, IViewModelFactory baseModelFactory)
        {
            _serviceProvider = serviceProvider;
            _baseModelFactory = baseModelFactory;
        }

        public ElectricLineShapeViewModel CreateElectricLineShape(PointShapeViewModel? start, PointShapeViewModel? end, ShapeStyleViewModel? style, bool isStroked = true, string name = "")
        {
            var lineShape = new ElectricLineShapeViewModel(_serviceProvider)
            {
                Name = name,
                State = ShapeStateFlags.Visible | ShapeStateFlags.Printable | ShapeStateFlags.Standalone,
                Properties = ImmutableArray.Create<PropertyViewModel>(),
                Style = style,
                IsStroked = isStroked,
                IsFilled = false,
                Start = start,
                End = end
            };

            return lineShape;
        }

        public ElectricLineShapeViewModel CreateElectricLineShape(double x1, double y1, double x2, double y2, ShapeStyleViewModel? style, bool isStroked = true, string name = "")
        {
            var lineShape = new ElectricLineShapeViewModel(_serviceProvider)
            {
                Name = name,
                State = ShapeStateFlags.Visible | ShapeStateFlags.Printable | ShapeStateFlags.Standalone,
                Properties = ImmutableArray.Create<PropertyViewModel>(),
                Style = style,
                IsStroked = isStroked,
                IsFilled = false,
                Start = _baseModelFactory.CreatePointShape(x1, y1),
                End = _baseModelFactory.CreatePointShape(x2, y2)
            };

            lineShape.Start.Owner = lineShape;
            lineShape.End.Owner = lineShape;

            return lineShape;
        }

        public ElectricLineShapeViewModel CreateElectricLineShape(double x, double y, ShapeStyleViewModel? style, bool isStroked = true, string name = "")
        {
            return CreateElectricLineShape(x, y, x, y, style, isStroked, name);
        }

        public ElectricSwitchShapeViewModel CreateElectricSwitchShape(double x1, double y1, double x2, double y2, ShapeStyleViewModel? style, string name = "")
        {
            var imageShape = new ElectricSwitchShapeViewModel(_serviceProvider)
            {
                Name = name,
                State = ShapeStateFlags.Visible | ShapeStateFlags.Printable | ShapeStateFlags.Standalone,
                Properties = ImmutableArray.Create<PropertyViewModel>(),
                Style = style,
                IsStroked = false,
                IsFilled = false,
                TopLeft = _baseModelFactory.CreatePointShape(x1, y1),
                BottomRight = _baseModelFactory.CreatePointShape(x2, y2)
            };

            imageShape.TopLeft.Owner = imageShape;
            imageShape.BottomRight.Owner = imageShape;

            return imageShape;
        }

        public ElectricSwitchShapeViewModel CreateElectricSwitchShape(double x, double y, ShapeStyleViewModel? style, string name = "")
        {
            return CreateElectricSwitchShape(x, y, x, y, style, name);
        }

        public ElectricSwitchShapeViewModel CreateElectricSwitchShape(PointShapeViewModel? topLeft, PointShapeViewModel? bottomRight, ShapeStyleViewModel? style, string name = "")
        {
            var imageShape = new ElectricSwitchShapeViewModel(_serviceProvider)
            {
                Name = name,
                State = ShapeStateFlags.Visible | ShapeStateFlags.Printable | ShapeStateFlags.Standalone,
                Properties = ImmutableArray.Create<PropertyViewModel>(),
                Style = style,
                IsStroked = false,
                IsFilled = false,
                TopLeft = topLeft,
                BottomRight = bottomRight
            };

            imageShape.TopLeft = topLeft;
            imageShape.BottomRight = bottomRight;

            return imageShape;
        }
    }
}
