using Autonoma.Domain.Electric;
using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Presentation.Controls.Electric
{
    public partial class GroundControl : UserControl
    {
        public GroundControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(30, 30)]
    [Pin(15, 0, Model.PinAlignment.Top)]
    public class GroundControlViewModel : VoltageControlViewModel
    {
        public GroundControlViewModel()
        {
        }

        public override VoltageControlType VoltageControlType => VoltageControlType.Ground;

        public override Conductivity Conductivity => Conductivity.Yes;

        public override GroundState GroundState => GroundState.Grounded;
    }
}
