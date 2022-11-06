#nullable enable
using System;
using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;
using Core2D.Model;
using Core2D.ViewModels;
using Core2D.ViewModels.Containers;
using Core2D.ViewModels.Shapes;
using Core2D.ViewModels.Style;

namespace Autonoma.UI.Shapes.Electric.ViewModels.Tools.Selection;

public class ElectricSwitchSelection
{
    private readonly IServiceProvider? _serviceProvider;
    private readonly LayerContainerViewModel _layer;
    private readonly ElectricSwitchShapeViewModel _image;
    private readonly ShapeStyleViewModel _styleViewModel;
    private PointShapeViewModel? _topLeftHelperPoint;
    private PointShapeViewModel? _bottomRightHelperPoint;

    public ElectricSwitchSelection(IServiceProvider? serviceProvider, LayerContainerViewModel layer, ElectricSwitchShapeViewModel shape, ShapeStyleViewModel style)
    {
        _serviceProvider = serviceProvider;
        _layer = layer;
        _image = shape;
        _styleViewModel = style;
    }

    public void ToStateBottomRight()
    {
        _topLeftHelperPoint = _serviceProvider.GetService<IViewModelFactory>()?.CreatePointShape();
        _bottomRightHelperPoint = _serviceProvider.GetService<IViewModelFactory>()?.CreatePointShape();

        if (_topLeftHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Add(_topLeftHelperPoint);
        }

        if (_bottomRightHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Add(_bottomRightHelperPoint);
        }
    }

    public void Move()
    {
        if (_topLeftHelperPoint is { } && _image.TopLeft is { })
        {
            _topLeftHelperPoint.X = _image.TopLeft.X;
            _topLeftHelperPoint.Y = _image.TopLeft.Y;
        }

        if (_bottomRightHelperPoint is { } && _image.BottomRight is { })
        {
            _bottomRightHelperPoint.X = _image.BottomRight.X;
            _bottomRightHelperPoint.Y = _image.BottomRight.Y;
        }

        _layer.RaiseInvalidateLayer();
    }

    public void Reset()
    {
        if (_topLeftHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Remove(_topLeftHelperPoint);
            _topLeftHelperPoint = null;
        }

        if (_bottomRightHelperPoint is { })
        {
            _layer.Shapes = _layer.Shapes.Remove(_bottomRightHelperPoint);
            _bottomRightHelperPoint = null;
        }

        _layer.RaiseInvalidateLayer();
    }
}
