using Autonoma.Domain;
using Autonoma.Domain.Electric;
using Autonoma.UI.Presentation.Attributes;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ReactiveUI.Fody.Helpers;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Controls.Electric
{
    public partial class SwitchControl : UserControl
    {
        public SwitchControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }

    [DefaultSize(50, 50)]
    [FixedSize]
    [Pin(0, 25, Model.PinAlignment.Left)]
    [Pin(50, 25, Model.PinAlignment.Right)]
    public class SwitchControlViewModel : VoltageControlViewModel
    {
        private static readonly Brush _defaultBrush = new SolidColorBrush(Colors.DarkGray);

        [Reactive]
        [Browsable(false)]
        public bool NotValid { get; set; }

        [Reactive]
        [Browsable(false)]
        public SwitchPosition Position { get; set; }

        [Browsable(false)]
        public override Conductivity Conductivity
        {
            get
            {
                if (Position == SwitchPosition.On) return Conductivity.Yes;
                else if (Position == SwitchPosition.Off) return Conductivity.No;
                return Conductivity.Unknown;
            }
        }

        public SwitchControlViewModel()
        {
        }

        public override void Update(DataPointInfo update)
        {
            var pos = SwitchPosition.Unknown;
            if (update != null)
            {
                if (update.Value.Value is int iv)
                    pos = (SwitchPosition)iv;
                Position = pos;
                NotValid = update.Value.Quality != DataQuality.Ok;
            }
        }
    }
}
