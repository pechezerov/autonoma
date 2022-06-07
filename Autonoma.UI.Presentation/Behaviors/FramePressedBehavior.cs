using Autonoma.UI.Presentation.Abstractions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Autonoma.UI.Presentation.Behaviors
{
    /// <summary>
    /// Обеспечивает взаимодействие с кадром
    /// </summary>
    public class FramePressedBehavior : Behavior<ItemsControl>
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
            if (AssociatedObject?.DataContext is not IFrame frame)
            {
                return;
            }

            if (e.Source is Control { DataContext: IPin })
            {
                return;
            }

            var info = e.GetCurrentPoint(AssociatedObject);
            var (x, y) = e.GetPosition(AssociatedObject);

            if (info.Properties.IsLeftButtonPressed)
            {
                frame.FrameLeftPressed(x, y);
            }
            else if (info.Properties.IsRightButtonPressed)
            {
                frame.FrameRightPressed(x, y);
            }
        }
    }
}