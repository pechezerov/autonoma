﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace Autonoma.UI.Presentation.Controls
{
    public class GridDecorator : Decorator
    {
        public static readonly StyledProperty<bool> EnableGridProperty =
            AvaloniaProperty.Register<GridDecorator, bool>(nameof(EnableGrid));

        public static readonly StyledProperty<double> GridCellWidthProperty =
            AvaloniaProperty.Register<GridDecorator, double>(nameof(GridCellWidth));

        public static readonly StyledProperty<double> GridCellHeightProperty =
            AvaloniaProperty.Register<GridDecorator, double>(nameof(GridCellHeight));

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

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == EnableGridProperty
                || change.Property == GridCellWidthProperty
                || change.Property == GridCellHeightProperty)
            {
                InvalidateVisual();
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (!EnableGrid)
            {
                return;
            }

            var cw = GridCellWidth;
            var ch = GridCellHeight;
            if (cw <= 0 || ch <= 0.0)
            {
                return;
            }

            var rect = Bounds;
            var thickness = 1.0;
            // rect = rect.Deflate(thickness * 0.5);

            var brush = new ImmutableSolidColorBrush(Color.FromArgb(255, 222, 222, 222));
            var pen = new ImmutablePen(brush, thickness);

            var ox = rect.X;
            var ex = rect.X + rect.Width;
            var oy = rect.Y;
            var ey = rect.Y + rect.Height;
            for (var x = ox + cw; x < ex; x += cw)
            {
                var p0 = new Point(x, oy);
                var p1 = new Point(x, ey);
                context.DrawLine(pen, p0, p1);
            }

            for (var y = oy + ch; y < ey; y += ch)
            {
                var p0 = new Point(ox, y);
                var p1 = new Point(ex, y);
                context.DrawLine(pen, p0, p1);
            }

            context.DrawRectangle(null, pen, rect);
        }
    }
}
