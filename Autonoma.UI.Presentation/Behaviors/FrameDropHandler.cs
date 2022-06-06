using System.Linq;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace Autonoma.UI.Presentation.Behaviors
{
    public class FrameDropHandler : DefaultDropHandler
    {
        public static readonly StyledProperty<IControl?> RelativeToProperty =
            AvaloniaProperty.Register<FrameDropHandler, IControl?>(nameof(RelativeTo));

        public IControl? RelativeTo
        {
            get => GetValue(RelativeToProperty) as Control;
            set => SetValue(RelativeToProperty, value);
        }

        private bool Validate(IFrame drawing, object? sender, DragEventArgs e, bool bExecute)
        {
            var relativeTo = RelativeTo ?? sender as IControl;
            if (relativeTo is null)
            {
                return false;
            }
            var point = GetPosition(relativeTo, e);

            if (relativeTo is FrameControl drawingNode)
            {
                point = SnapHelper.Snap(point, drawingNode.SnapX, drawingNode.SnapY, drawingNode.EnableSnap);
            }

            if (e.Data.Contains(DataFormats.Text))
            {
                var text = e.Data.GetText();

                if (bExecute)
                {
                    if (text is { })
                    {
                        // TODO: text
                    }
                }

                return true;
            }

            foreach (var format in e.Data.GetDataFormats())
            {
                var data = e.Data.Get(format);

                switch (data)
                {
                    case IElementPrototype template:
                        {
                            if (bExecute)
                            {
                                var node = drawing.Clone(template.Template);
                                if (node is { })
                                {
                                    node.Parent = drawing;
                                    node.Move(point.X, point.Y);
                                    drawing.Nodes?.Add(node);
                                }
                            }
                            return true;
                        }
                }
            }

            if (e.Data.Contains(DataFormats.FileNames))
            {
                // ReSharper disable once UnusedVariable
                var files = e.Data.GetFileNames()?.ToArray();
                if (bExecute)
                {
                    // TODO: files, point.X, point.Y
                }

                return true;
            }

            return false;
        }

        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (targetContext is IFrame drawing)
            {
                return Validate(drawing, sender, e, false);
            }

            return false;
        }

        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (targetContext is IFrame drawing)
            {
                return Validate(drawing, sender, e, true);
            }

            return false;
        }
    }
}