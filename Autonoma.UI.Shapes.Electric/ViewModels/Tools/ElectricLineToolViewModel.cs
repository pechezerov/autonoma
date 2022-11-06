#nullable enable
using System;
using System.Collections.Generic;
using Autonoma.UI.Shapes.Electric.ViewModels.Shapes;
using Autonoma.UI.Shapes.Electric.ViewModels.Tools.Selection;
using Core2D.Model;
using Core2D.Model.Editor;
using Core2D.Model.Input;
using Core2D.ViewModels;
using Core2D.ViewModels.Editor;
using Core2D.ViewModels.Shapes;
using Core2D.ViewModels.Style;

namespace Autonoma.UI.Shapes.Electric.ViewModels.Tools;

public partial class ElectricLineToolViewModel : ViewModelBase, IEditorTool
{
    public enum State { Start, End }
    private State _currentState = State.Start;
    private ElectricLineShapeViewModel? _electricLine;
    private ElectricLineSelection? _selection;

    public string Title => "ElectricLine";

    public ElectricLineToolViewModel(IServiceProvider? serviceProvider) : base(serviceProvider)
    {
    }

    public override object Copy(IDictionary<object, object>? shared)
    {
        throw new NotImplementedException();
    }

    public void BeginDown(InputArgs args)
    {
        var factory = ServiceProvider.GetService<IAutonomaViewModelFactory>();
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        var selection = ServiceProvider.GetService<ISelectionService>();
        var viewModelFactory = ServiceProvider.GetService<IViewModelFactory>();

        if (factory is null || editor?.Project?.Options is null || selection is null || viewModelFactory is null)
        {
            return;
        }

        (var x, var y) = args;
        (var sx, var sy) = selection.TryToSnap(args);
        switch (_currentState)
        {
            case State.Start:
                {
                    editor.IsToolIdle = false;
                    var style = editor.Project.CurrentStyleLibrary?.Selected is { } ?
                        editor.Project.CurrentStyleLibrary.Selected :
                        viewModelFactory.CreateShapeStyle(ProjectEditorConfiguration.DefaultStyleName);
                    _electricLine = factory.CreateElectricLineShape(
                        (double)sx, (double)sy,
                        (ShapeStyleViewModel)style.Copy(null),
                        editor.Project.Options.DefaultIsStroked);

                    editor.SetShapeName(_electricLine);

                    if (editor.Project.Options.TryToConnect)
                    {
                        var result = selection.TryToGetConnectionPoint((double)sx, (double)sy);
                        if (result is { })
                        {
                            _electricLine.Start = result;
                        }
                        else
                        {
                            selection.TryToSplitLine(x, y, _electricLine.Start);
                        }
                    }
                    editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Add(_electricLine);
                    editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                    ToStateEnd();
                    Move(_electricLine);
                    _currentState = State.End;
                }
                break;
            case State.End:
                {
                    if (_electricLine is { })
                    {
                        _electricLine.End.X = (double)sx;
                        _electricLine.End.Y = (double)sy;

                        if (editor.Project.Options.TryToConnect)
                        {
                            var result = selection.TryToGetConnectionPoint((double)sx, (double)sy);
                            if (result is { })
                            {
                                _electricLine.End = result;
                            }
                            else
                            {
                                selection.TryToSplitLine(x, y, _electricLine.End);
                            }
                        }

                        editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_electricLine);
                        Finalize(_electricLine);
                        editor.Project.AddShape(editor.Project.CurrentContainer.CurrentLayer, _electricLine);

                        Reset();
                    }
                }
                break;
        }
    }

    public void BeginUp(InputArgs args)
    {
    }

    public void EndDown(InputArgs args)
    {
        switch (_currentState)
        {
            case State.Start:
                break;
            case State.End:
                Reset();
                break;
        }
    }

    public void EndUp(InputArgs args)
    {
    }

    public void Move(InputArgs args)
    {
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        var selection = ServiceProvider.GetService<ISelectionService>();
        (var sx, var sy) = selection.TryToSnap(args);
        switch (_currentState)
        {
            case State.Start:
                {
                    if (editor.Project.Options.TryToConnect)
                    {
                        selection.TryToHoverShape((double)sx, (double)sy);
                    }
                }
                break;
            case State.End:
                {
                    if (_electricLine is { })
                    {
                        if (editor.Project.Options.TryToConnect)
                        {
                            selection.TryToHoverShape((double)sx, (double)sy);
                        }
                        _electricLine.End.X = (double)sx;
                        _electricLine.End.Y = (double)sy;
                        editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                        Move(_electricLine);
                    }
                }
                break;
        }
    }

    public void ToStateEnd()
    {
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        _selection = new ElectricLineSelection(
            ServiceProvider,
            editor.Project.CurrentContainer.HelperLayer,
            _electricLine,
            editor.PageState.HelperStyle);

        _selection.ToStateEnd();
    }

    public void Move(BaseShapeViewModel shape)
    {
        _selection?.Move();
    }

    public void Finalize(BaseShapeViewModel shape)
    {
    }

    public void Reset()
    {
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();

        if (editor is null)
        {
            return;
        }

        switch (_currentState)
        {
            case State.Start:
                break;
            case State.End:
                {
                    editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_electricLine);
                    editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                }
                break;
        }

        _currentState = State.Start;

        if (_selection is { })
        {
            _selection.Reset();
            _selection = null;
        }

        editor.IsToolIdle = true;
    }
}
