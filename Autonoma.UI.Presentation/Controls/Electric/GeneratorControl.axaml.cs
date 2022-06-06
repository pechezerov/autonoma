using Autonoma.Domain.Electric;
using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.Presentation.Controls.Electric
{
    public partial class GeneratorControl : UserControl
    {
        public GeneratorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(50, 50)]
    [Pin(25, 0, Model.PinAlignment.Top)]
    public class GeneratorControlViewModel : VoltageControlViewModel
    {
        public GeneratorControlViewModel()
        {
        }

        public override VoltageControlType VoltageControlType => VoltageControlType.Source;

        public override Conductivity Conductivity => Conductivity.Yes;

        public override GroundState GroundState => GroundState.Grounded;
    }
}
