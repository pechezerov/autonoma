using System;
using System.ComponentModel;
using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Xaml.Interactivity;

namespace Autonoma.UI.Presentation.Behaviors
{
    public class ElementSelectedBehavior : Behavior<ItemsControl>
    {
        private IDisposable? _isEditModeDisposable;
        private IDisposable? _dataContextDisposable;
        private INotifyPropertyChanged? _framePropertyChanged;
        private IFrame? _frame;

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is not null)
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
                        if (x is IFrame drawingNode)
                        {
                            if (_frame == drawingNode)
                            {
                                if (_framePropertyChanged != null)
                                {
                                    _framePropertyChanged.PropertyChanged -= Frame_PropertyChanged;
                                }
                            }

                            RemoveSelectedPseudoClasses(AssociatedObject);

                            _frame = drawingNode;

                            if (_frame is INotifyPropertyChanged notifyPropertyChanged)
                            {
                                _framePropertyChanged = notifyPropertyChanged;
                                _framePropertyChanged.PropertyChanged += Frame_PropertyChanged;
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

            if (AssociatedObject is not null)
            {
                if (_framePropertyChanged is not null)
                {
                    _framePropertyChanged.PropertyChanged -= Frame_PropertyChanged;
                }

                _isEditModeDisposable?.Dispose();
                _dataContextDisposable?.Dispose();
            }
        }

        private void Frame_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (AssociatedObject?.DataContext is not IFrame)
            {
                return;
            }

            if (e.PropertyName == nameof(IFrame.SelectedNodes) || e.PropertyName == nameof(IFrame.SelectedConnectors))
            {
                if (_frame is not null)
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
                if (container.ContainerControl is not { DataContext: IElement element } containerControl)
                {
                    continue;
                }

                if (_frame is { } && _frame.SelectedNodes is { } && _frame.SelectedNodes.Contains(element))
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
                if (container.ContainerControl is not { DataContext: IElement } containerControl)
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
