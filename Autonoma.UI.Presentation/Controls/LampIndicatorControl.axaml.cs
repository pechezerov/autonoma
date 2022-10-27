using Autonoma.Domain;
using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Controls
{
    public partial class LampIndicatorControl : UserControl
    {
        public LampIndicatorControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(20, 20)]
    public class LampIndicatorControlViewModel : LooklessControlViewModel
    {
        private static readonly Brush _defaultBrush = new SolidColorBrush(Color.Parse("#444"));

        [Reactive]
        [Connected]
        public DataValue? Value { get; set; }

        [Reactive]
        public string ColorText { get; set; } = "#0f0";

        [Reactive]
        [Browsable(false)]
        public Brush IndicationBrush { get; set; } = _defaultBrush;

        public LampIndicatorControlViewModel()
        {
        }

        public override void Update(DataPointInfo update)
        {
            Value = update.Value;

            if (Value.ValueInt() != 0)
            {
                if (Color.TryParse(ColorText, out var color))
                    IndicationBrush = new SolidColorBrush(color);
            }
        }
    }
}
