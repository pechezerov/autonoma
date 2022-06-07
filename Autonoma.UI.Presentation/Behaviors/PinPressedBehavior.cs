using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Autonoma.UI.Presentation.Behaviors
{
    public class PinPressedBehavior : Behavior<ContentPresenter>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not null)
            {
                AssociatedObject.AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is not null)
            {
                AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, Pressed);
            }
        }

        private void Pressed(object? sender, PointerPressedEventArgs e)
        {
            if (AssociatedObject?.DataContext is not IPin pin)
            {
                return;
            }

            if (!AssociatedObject.GetValue(FrameControl.IsEditModeProperty))
            {
                return;
            }

            if (pin.Parent is not { } nodeViewModel)
            {
                return;
            }

            if (nodeViewModel.Parent is IFrame frame)
            {
                var info = e.GetCurrentPoint(AssociatedObject);

                if (info.Properties.IsLeftButtonPressed)
                {
                    var showWhenMoving = info.Pointer.Type == PointerType.Mouse;

                    frame.ConnectorLeftPressed(pin, showWhenMoving);

                    e.Handled = true;
                }
            }
        }
    }
}