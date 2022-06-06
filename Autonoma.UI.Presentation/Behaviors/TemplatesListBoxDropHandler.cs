using Autonoma.UI.Presentation.Abstractions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;

namespace Autonoma.UI.Presentation.Behaviors
{
    public class TemplatesListBoxDropHandler : DropHandlerBase
    {
        private bool Validate<T>(ListBox listBox, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : IElementPrototype
        {
            if (sourceContext is not T sourceItem
                || targetContext is not IElementPrototypesHost nodeTemplatesHost
                || nodeTemplatesHost.Prototypes is null
                || listBox.GetVisualAt(e.GetPosition(listBox)) is not IControl targetControl
                || targetControl.DataContext is not T targetItem)
            {
                return false;
            }

            var sourceIndex = nodeTemplatesHost.Prototypes.IndexOf(sourceItem);
            var targetIndex = nodeTemplatesHost.Prototypes.IndexOf(targetItem);

            if (sourceIndex < 0 || targetIndex < 0)
            {
                return false;
            }

            if (e.DragEffects == DragDropEffects.Copy)
            {
                return false;
            }

            if (e.DragEffects == DragDropEffects.Move)
            {
                if (bExecute)
                {
                    MoveItem(nodeTemplatesHost.Prototypes, sourceIndex, targetIndex);
                }
                return true;
            }

            if (e.DragEffects == DragDropEffects.Link)
            {
                if (bExecute)
                {
                    SwapItem(nodeTemplatesHost.Prototypes, sourceIndex, targetIndex);
                }
                return true;
            }

            return false;
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is ListBox listBox)
            {
                return Validate<IElementPrototype>(listBox, e, sourceContext, targetContext, false);
            }
            return false;
        }

        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is ListBox listBox)
            {
                return Validate<IElementPrototype>(listBox, e, sourceContext, targetContext, true);
            }
            return false;
        }
    }
}