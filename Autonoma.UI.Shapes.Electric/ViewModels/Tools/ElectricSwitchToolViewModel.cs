#nullable enable
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

public partial class ElectricSwitchToolViewModel : ViewModelBase, IEditorTool
{
    public enum State { TopLeft, BottomRight }
    private State _currentState = State.TopLeft;
    private ElectricSwitchShapeViewModel? _switch;
    private ElectricSwitchSelection? _selection;

    public string Title => "ElectricSwitch";

    public ElectricSwitchToolViewModel(IServiceProvider? serviceProvider) : base(serviceProvider)
    {
    }

    public override object Copy(IDictionary<object, object>? shared)
    {
        throw new NotImplementedException();
    }

    public async void BeginDown(InputArgs args)
    {
        var factory = ServiceProvider.GetService<IAutonomaViewModelFactory>();
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        var selection = ServiceProvider.GetService<ISelectionService>();
        var viewModelFactory = ServiceProvider.GetService<IViewModelFactory>();

        if (factory is null || editor?.Project?.Options is null || selection is null || viewModelFactory is null)
        {
            return;
        }

        (decimal sx, decimal sy) = selection.TryToSnap(args);
        switch (_currentState)
        {
            case State.TopLeft:
                {
                    editor.IsToolIdle = false;

                    var style = editor.Project.CurrentStyleLibrary?.Selected is { } ?
                        editor.Project.CurrentStyleLibrary.Selected :
                        viewModelFactory.CreateShapeStyle(ProjectEditorConfiguration.DefaultStyleName);
                    _switch = factory.CreateElectricSwitchShape(
                        (double)sx, (double)sy,
                        (ShapeStyleViewModel)style.Copy(null));

                    editor.SetShapeName(_switch);

                    var result = selection.TryToGetConnectionPoint((double)sx, (double)sy);
                    if (result is { })
                    {
                        _switch.TopLeft = result;
                    }

                    editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Add(_switch);
                    editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                    ToStateBottomRight();
                    Move(_switch);
                    _currentState = State.BottomRight;
                }
                break;
            case State.BottomRight:
                {
                    if (_switch is { })
                    {
                        _switch.BottomRight.X = (double)sx;
                        _switch.BottomRight.Y = (double)sy;

                        var result = selection.TryToGetConnectionPoint((double)sx, (double)sy);
                        if (result is { })
                        {
                            _switch.BottomRight = result;
                        }

                        editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_switch);
                        Finalize(_switch);
                        editor.Project.AddShape(editor.Project.CurrentContainer.CurrentLayer, _switch);

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
            case State.TopLeft:
                break;
            case State.BottomRight:
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
        (decimal sx, decimal sy) = selection.TryToSnap(args);
        switch (_currentState)
        {
            case State.TopLeft:
                if (editor.Project.Options.TryToConnect)
                {
                    selection.TryToHoverShape((double)sx, (double)sy);
                }
                break;
            case State.BottomRight:
                {
                    if (_switch is { })
                    {
                        if (editor.Project.Options.TryToConnect)
                        {
                            selection.TryToHoverShape((double)sx, (double)sy);
                        }
                        _switch.BottomRight.X = (double)sx;
                        _switch.BottomRight.Y = (double)sy;
                        editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                        Move(_switch);
                    }
                }
                break;
        }
    }

    public void ToStateBottomRight()
    {
        var editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        _selection = new ElectricSwitchSelection(
            ServiceProvider,
            editor.Project.CurrentContainer.HelperLayer,
            _switch,
            editor.PageState.HelperStyle);

        _selection.ToStateBottomRight();
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
            case State.TopLeft:
                break;
            case State.BottomRight:
                {
                    editor.Project.CurrentContainer.WorkingLayer.Shapes = editor.Project.CurrentContainer.WorkingLayer.Shapes.Remove(_switch);
                    editor.Project.CurrentContainer.WorkingLayer.RaiseInvalidateLayer();
                }
                break;
        }

        _currentState = State.TopLeft;

        if (_selection is { })
        {
            _selection.Reset();
            _selection = null;
        }

        editor.IsToolIdle = true;
    }
}
