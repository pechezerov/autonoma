﻿#nullable enable
using System;
using System.Collections.Generic;
using Core2D.Model.Editor;
using Core2D.Model.Renderer;
using Core2D.ViewModels.Shapes;
using Core2D.Spatial;
using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;

namespace Autonoma.UI.Shapes.Electric.Bounds;

public class ElectricLineBounds : IBounds
{
    public Type TargetType => typeof(ElectricLineShapeViewModel);

    public PointShapeViewModel? TryToGetPoint(BaseShapeViewModel shape, Point2 target, double radius, double scale, IDictionary<Type, IBounds> registered)
    {
        if (shape is not ElectricLineShapeViewModel line)
        {
            throw new ArgumentNullException(nameof(shape));
        }

        if (line.Start is null || line.End is null)
        {
            return null;
        }

        var pointHitTest = registered[typeof(PointShapeViewModel)];

        if (pointHitTest.TryToGetPoint(line.Start, target, radius, scale, registered) is { })
        {
            return line.Start;
        }

        if (pointHitTest.TryToGetPoint(line.End, target, radius, scale, registered) is { })
        {
            return line.End;
        }

        return null;
    }

    public bool Contains(BaseShapeViewModel shape, Point2 target, double radius, double scale, IDictionary<Type, IBounds> registered)
    {
        if (shape is not ElectricLineShapeViewModel line)
        {
            throw new ArgumentNullException(nameof(shape));
        }

        if (line.Start is null || line.End is null)
        {
            return false;
        }

        Point2 a;
        Point2 b;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (line.State.HasFlag(ShapeStateFlags.Size) && scale != 1.0)
        {
            a = new Point2(line.Start.X * scale, line.Start.Y * scale);
            b = new Point2(line.End.X * scale, line.End.Y * scale);
        }
        else
        {
            a = new Point2(line.Start.X, line.Start.Y);
            b = new Point2(line.End.X, line.End.Y);
        }

        var nearest = target.NearestOnLine(a, b);
        var distance = target.DistanceTo(nearest);
        return distance < radius;
    }

    public bool Overlaps(BaseShapeViewModel shape, Rect2 target, double radius, double scale, IDictionary<Type, IBounds> registered)
    {
        if (shape is not ElectricLineShapeViewModel line)
        {
            throw new ArgumentNullException(nameof(shape));
        }

        if (line.Start is null || line.End is null)
        {
            return false;
        }

        Point2 a;
        Point2 b;
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (line.State.HasFlag(ShapeStateFlags.Size) && scale != 1.0)
        {
            a = new Point2(line.Start.X * scale, line.Start.Y * scale);
            b = new Point2(line.End.X * scale, line.End.Y * scale);
        }
        else
        {
            a = new Point2(line.Start.X, line.Start.Y);
            b = new Point2(line.End.X, line.End.Y);
        }

        return Line2.LineIntersectsWithRect(a, b, target, out var _, out var _, out var _, out var _);
    }
}
