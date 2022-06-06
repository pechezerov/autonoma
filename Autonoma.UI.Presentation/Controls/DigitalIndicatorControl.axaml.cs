using Autonoma.Domain;
using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI.Fody.Helpers;

namespace Autonoma.UI.Presentation.Controls
{
    public partial class DigitalIndicatorControl : UserControl
    {
        public DigitalIndicatorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(100, 25)]
    public class DigitalIndicatorControlViewModel : LooklessControlViewModel
    {
        [Reactive]
        public string Value { get; set; } = "-";

        public DigitalIndicatorControlViewModel()
        {
        }

        public override void Update(DataPointInfo update)
        {
            Value = update.Value.ValueString();
        }
    }
}
