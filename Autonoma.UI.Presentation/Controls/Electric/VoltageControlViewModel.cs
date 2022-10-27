using Autonoma.Domain;
using Autonoma.Domain.Electric;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Controls.Electric
{
    public abstract class VoltageControlViewModel : LooklessControlViewModel
    {
        [Reactive]
        public VoltageClass VoltageClass { get; set; }

        public virtual VoltageControlType VoltageControlType => VoltageControlType.Usual;

        [Reactive]
        public virtual VoltageState VoltageState { get; set; }

        [Reactive]
        public virtual GroundState GroundState { get; set; }

        [Browsable(false)]
        public virtual Conductivity Conductivity => Conductivity.Unknown;

        public override void Update(DataPointInfo update)
        {
        }
    }
}