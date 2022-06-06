using Autonoma.UI.Presentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Autonoma.UI.FrameEditor.Views.Documents
{
    public partial class DocumentView : UserControl
    {
        public static readonly StyledProperty<FrameZoomBorder?> ZoomControlProperty =
            AvaloniaProperty.Register<DocumentView, FrameZoomBorder?>(nameof(ZoomControl));

        public static readonly StyledProperty<FrameControl?> FrameControlProperty =
            AvaloniaProperty.Register<DocumentView, FrameControl?>(nameof(FrameControl));

        public DocumentView()
        {
            this.InitializeComponent();
            this.AttachedToVisualTree += DocumentView_AttachedToVisualTree;
        }

        // workaround, решающий следующую проблему:
        // - KeyBinding'и не работают, если целевой элемент не в фокусе
        // - после размещения в док-панели элемент управления почему-то не получает фокус даже после вызова SetFocusedDockable()
        // - фокус в этом случае обретается только после клика за пределами дочернего Canvas, что неудобоваримо
        private void DocumentView_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            this.Focus();
        }

        public FrameZoomBorder? ZoomControl
        {
            get => GetValue(ZoomControlProperty);
            set => SetValue(ZoomControlProperty, value);
        }

        public FrameControl? FrameControl
        {
            get => GetValue(FrameControlProperty);
            set => SetValue(FrameControlProperty, value);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
