#nullable enable
using Core2D.ViewModels;
using Core2D.ViewModels.Shapes;

namespace Autonoma.UI.Shapes.Electric.ViewModels.Shapes;

public partial class ElectricLineShapeViewModel : LineShapeViewModel
{
    public ElectricLineShapeViewModel(IServiceProvider? serviceProvider) : base(serviceProvider, typeof(ElectricLineShapeViewModel))
    {
    }

    public override object Copy(IDictionary<object, object>? shared)
    {
        var copy = new ElectricLineShapeViewModel(ServiceProvider)
        {
            Name = Name,
            State = State,
            Style = _style?.CopyShared(shared),
            IsStroked = IsStroked,
            IsFilled = IsFilled,
            Properties = _properties.CopyShared(shared).ToImmutable(),
            Record = _record,
            Start = _start?.CopyShared(shared),
            End = _end?.CopyShared(shared)
        };

        return copy;
    }
}
