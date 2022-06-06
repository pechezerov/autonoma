using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Xaml.Interactivity;
using System;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Behaviors
{
    public partial class ConnectorsSelectedBehavior : Behavior<ItemsControl>
    {
        private IDisposable? _isEditModeDisposable;
        private IDisposable? _dataContextDisposable;
        private INotifyPropertyChanged? _drawingNodePropertyChanged;
        private IFrame? _frame;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is { })
            {
                _isEditModeDisposable = AssociatedObject.GetObservable(FrameControl.IsEditModeProperty)
                    .Subscribe(x =>
                    {
                        if (x == false)
                        {
                            RemoveSelectedPseudoClasses(AssociatedObject);
                        }
                    });

                _dataContextDisposable = AssociatedObject
                    .GetObservable(StyledElement.DataContextProperty)
                    .Subscribe(x =>
                    {
                        if (x is IFrame frame)
                        {
                            if (_frame == frame)
                            {
                                if (_drawingNodePropertyChanged != null)
                                {
                                    _drawingNodePropertyChanged.PropertyChanged -= DrawingNode_PropertyChanged;
                                }
                            }

                            RemoveSelectedPseudoClasses(AssociatedObject);

                            _frame = frame;

                            if (_frame is INotifyPropertyChanged notifyPropertyChanged)
                            {
                                _drawingNodePropertyChanged = notifyPropertyChanged;
                                _drawingNodePropertyChanged.PropertyChanged += DrawingNode_PropertyChanged;
                            }
                        }
                        else
                        {
                            RemoveSelectedPseudoClasses(AssociatedObject);
                        }
                    });
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is { })
            {
                if (_drawingNodePropertyChanged is { })
                {
                    _drawingNodePropertyChanged.PropertyChanged -= DrawingNode_PropertyChanged;
                }

                _isEditModeDisposable?.Dispose();
                _dataContextDisposable?.Dispose();
            }
        }

        private void DrawingNode_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (AssociatedObject?.DataContext is not IFrame)
            {
                return;
            }

            if (e.PropertyName == nameof(IFrame.SelectedNodes) || e.PropertyName == nameof(IFrame.SelectedConnectors))
            {
                if (_frame is { })
                {
                    if (_frame.SelectedNodes is { Count: > 0 } || _frame.SelectedConnectors is { Count: > 0 })
                    {
                        AddSelectedPseudoClasses(AssociatedObject);
                    }
                    else
                    {
                        RemoveSelectedPseudoClasses(AssociatedObject);
                    }
                }
            }
        }

        private void AddSelectedPseudoClasses(ItemsControl itemsControl)
        {
            foreach (var container in itemsControl.ItemContainerGenerator.Containers)
            {
                if (container.ContainerControl is not { DataContext: IConnector connector } containerControl)
                {
                    continue;
                }

                if (_frame is { } && _frame.SelectedConnectors is { } && _frame.SelectedConnectors.Contains(connector))
                {
                    if (containerControl is ContentPresenter { Child: { } child })
                    {
                        if (child.Classes is IPseudoClasses pseudoClasses)
                        {
                            pseudoClasses.Add(":selected");
                        }
                    }
                }
                else
                {
                    if (containerControl is ContentPresenter { Child: { } child })
                    {
                        if (child.Classes is IPseudoClasses pseudoClasses)
                        {
                            pseudoClasses.Remove(":selected");
                        }
                    }
                }
            }
        }

        private static void RemoveSelectedPseudoClasses(ItemsControl itemsControl)
        {
            foreach (var container in itemsControl.ItemContainerGenerator.Containers)
            {
                if (container.ContainerControl is not { DataContext: IConnector } containerControl)
                {
                    continue;
                }

                if (containerControl is ContentPresenter { Child: { } child })
                {
                    if (child.Classes is IPseudoClasses pseudoClasses)
                    {
                        pseudoClasses.Remove(":selected");
                    }
                }
            }
        }
    }
}