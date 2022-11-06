#nullable enable
using System;
using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;
using Core2D.Model;
using Core2D.ViewModels;
using Core2D.ViewModels.Containers;
using Core2D.ViewModels.Shapes;
using Core2D.ViewModels.Style;

namespace Autonoma.UI.Shapes.Electric.ViewModels.Tools.Selection;

public class ElectricLineSelection
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly LayerContainerViewModel _layer;
    private readonly ElectricLineShapeViewModel _electricline;
    private readonly ShapeStyleViewModel _styleViewModel;
    private PointShapeViewModel? _startHelperPoint;
    private PointShapeViewModel? _endHelperPoint;

    public ElectricLineSelection(IServiceProvider? serviceProvider, LayerContainerViewModel layer, ElectricLineShapeViewModel shape, ShapeStyleViewModel style)
    {
        _serviceProvider = serviceProvider;
        _layer = layer;
        _electricline = shape;
        _styleViewModel = style;
    }

    public void ToStateEnd()
    {
        _startHelperPoint = _serviceProvider.GetService<IViewModelFactory>()?.CreatePointShape();
        _endHelperPoint = _serviceProvider.GetService<IViewModelFactory>()?.CreatePointShape();

        if (_startHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Add(_startHelperPoint);
        }

        if (_endHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Add(_endHelperPoint);
        }
    }

    public void Move()
    {
        if (_startHelperPoint is { } && _electricline.Start is { })
        {
            _startHelperPoint.X = _electricline.Start.X;
            _startHelperPoint.Y = _electricline.Start.Y;
        }

        if (_endHelperPoint is { } && _electricline.End is { })
        {
            _endHelperPoint.X = _electricline.End.X;
            _endHelperPoint.Y = _electricline.End.Y;
        }

        _layer.RaiseInvalidateLayer();
    }

    public void Reset()
    {
        if (_startHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Remove(_startHelperPoint);
            _startHelperPoint = null;
        }

        if (_endHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Remove(_endHelperPoint);
            _endHelperPoint = null;
        }

        _layer.RaiseInvalidateLayer();
    }
}
