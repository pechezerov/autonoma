using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Autonoma.UI.Presentation.Controls
{
    public class FrameControl : TemplatedControl
    {
        public static readonly AttachedProperty<bool> IsEditModeProperty =
            AvaloniaProperty.RegisterAttached<IAvaloniaObject, bool>("IsEditMode", typeof(FrameControl), true, true);

        public static readonly StyledProperty<Control?> InputSourceProperty =
            AvaloniaProperty.Register<FrameControl, Control?>(nameof(InputSource));

        public static readonly StyledProperty<Canvas?> AdornerCanvasProperty =
            AvaloniaProperty.Register<FrameControl, Canvas?>(nameof(AdornerCanvas));

        public static readonly StyledProperty<bool> EnableSnapProperty =
            AvaloniaProperty.Register<FrameControl, bool>(nameof(EnableSnap));

        public static readonly StyledProperty<double> SnapXProperty =
            AvaloniaProperty.Register<FrameControl, double>(nameof(SnapX), 1.0);

        public static readonly StyledProperty<double> SnapYProperty =
            AvaloniaProperty.Register<FrameControl, double>(nameof(SnapY), 1.0);

        public static readonly StyledProperty<bool> EnableGridProperty =
            AvaloniaProperty.Register<FrameControl, bool>(nameof(EnableGrid));

        public static readonly StyledProperty<double> GridCellWidthProperty =
            AvaloniaProperty.Register<FrameControl, double>(nameof(GridCellWidth));

        public static readonly StyledProperty<double> GridCellHeightProperty =
            AvaloniaProperty.Register<FrameControl, double>(nameof(GridCellHeight));

        public Control? InputSource
        {
            get => GetValue(InputSourceProperty);
            set => SetValue(InputSourceProperty, value);
        }

        public Canvas? AdornerCanvas
        {
            get => GetValue(AdornerCanvasProperty);
            set => SetValue(AdornerCanvasProperty, value);
        }

        public bool EnableSnap
        {
            get => GetValue(EnableSnapProperty);
            set => SetValue(EnableSnapProperty, value);
        }

        public double SnapX
        {
            get => GetValue(SnapXProperty);
            set => SetValue(SnapXProperty, value);
        }

        public double SnapY
        {
            get => GetValue(SnapYProperty);
            set => SetValue(SnapYProperty, value);
        }

        public bool EnableGrid
        {
            get => GetValue(EnableGridProperty);
            set => SetValue(EnableGridProperty, value);
        }

        public double GridCellWidth
        {
            get => GetValue(GridCellWidthProperty);
            set => SetValue(GridCellWidthProperty, value);
        }

        public double GridCellHeight
        {
            get => GetValue(GridCellHeightProperty);
            set => SetValue(GridCellHeightProperty, value);
        }

        public static bool GetIsEditMode(IAvaloniaObject obj)
        {
            return obj.GetValue(IsEditModeProperty);
        }

        public static void SetIsEditMode(IAvaloniaObject obj, bool value)
        {
            obj.SetValue(IsEditModeProperty, value);
        }

        public FrameControl()
        {

        }
    }
}
