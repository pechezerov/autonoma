#nullable enable
using Core2D.Model;
using Core2D.Model.Renderer;
using Core2D.ViewModels.Shapes;
using System.ComponentModel;
using Autonoma.UI.ViewModels;
using Core2D.ViewModels;
using AutoNotifyAttribute = Autonoma.UI.ViewModels.AutoNotifyAttribute;

namespace Autonoma.UI.Shapes.Electric.ViewModels.Shapes;

public partial class ElectricSwitchShapeViewModel : ImageShapeViewModel
{
    public static VoltageClass[] VoltageClassValues { get; } = (VoltageClass[])Enum.GetValues(typeof(VoltageClass));
    public static SwitchType[] SwitchTypeValues { get; } = (SwitchType[])Enum.GetValues(typeof(SwitchType));
    public static SwitchState[] SwitchStateValues { get; } = (SwitchState[])Enum.GetValues(typeof(SwitchState));

    [AutoNotify] protected VoltageClass _voltageClass = VoltageClass.kV110;
    [AutoNotify] protected SwitchType _switchType = SwitchType.Q;
    [AutoNotify] protected SwitchState _switchState = SwitchState.On;
    [AutoNotify] protected string _stylePrefix = "FSK";

    public ElectricSwitchShapeViewModel(IServiceProvider? serviceProvider) : base(serviceProvider, typeof(ElectricSwitchShapeViewModel))
    {
        this.PropertyChanged += ElectricSwitchShapeViewModel_PropertyChanged;
        Key = GetImageKey();
    }

    private string? GetImageKey()
    {
        return $"Images\\{_stylePrefix}_{(int)_voltageClass}_{_switchType.ToString().ToUpper()}_V_{_switchState.ToString().ToUpper()}.png";
    }

    private void ElectricSwitchShapeViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "SwitchState"
            || e.PropertyName == "SwitchType"
            || e.PropertyName == "VoltageClass"
            || e.PropertyName == "StylePrefix")
            Key = GetImageKey();
    }

    public override object Copy(IDictionary<object, object>? shared)
    {
        var copy = new ElectricSwitchShapeViewModel(ServiceProvider)
        {
            Name = Name,
            State = State,
            Style = _style?.CopyShared(shared),
            IsStroked = IsStroked,
            IsFilled = IsFilled,
            Properties = _properties.CopyShared(shared).ToImmutable(),
            Record = _record,
            TopLeft = _topLeft?.CopyShared(shared),
            BottomRight = _bottomRight?.CopyShared(shared),
            Key = Key
        };

        return copy;
    }

    public override void DrawShape(object? dc, IShapeRenderer? renderer, ISelection? selection)
    {
        if (State.HasFlag(ShapeStateFlags.Visible))
        {
            renderer?.DrawImage(dc, this, Style);
        }
    }
}

public enum SwitchState
{
    Unknown,
    On,
    Off,
    Error
}

public enum SwitchType
{
    Q,
    QS,
    QK,
    QF,
    QR
}

public enum VoltageClass
{
    kV04 = 4,
    kV10 = 10,
    kV35 = 35,
    kV110 = 110,
    kV220 = 220,
    kV330 = 330,
    kV400 = 400,
    kV500 = 500,
    kV750 = 750,
    kV1150 = 1150
}
