using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Controls;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;
using System;
using System.ComponentModel;

namespace Autonoma.UI.Presentation.Behaviors
{
    /// <summary>
    /// Обеспечивает взаимодействие с элементами, расположенными на кадре:
    /// - выделение
    /// - перетаскивание (и поддержку привязку к сетке при перетаскивании)
    /// </summary>
    public class FrameSelectionBehavior : Behavior<ItemsControl>
    {
        public static readonly StyledProperty<Control?> InputSourceProperty =
            AvaloniaProperty.Register<FrameSelectionBehavior, Control?>(nameof(InputSource));

        public static readonly StyledProperty<Canvas?> AdornerCanvasProperty =
            AvaloniaProperty.Register<FrameSelectionBehavior, Canvas?>(nameof(AdornerCanvas));

        public static readonly StyledProperty<bool> EnableSnapProperty =
            AvaloniaProperty.Register<FrameSelectionBehavior, bool>(nameof(EnableSnap));

        public static readonly StyledProperty<double> SnapXProperty =
            AvaloniaProperty.Register<FrameSelectionBehavior, double>(nameof(SnapX), 1.0);

        public static readonly StyledProperty<double> SnapYProperty =
            AvaloniaProperty.Register<FrameSelectionBehavior, double>(nameof(SnapY), 1.0);

        private IDisposable? _isEditModeDisposable;
        private IDisposable? _dataContextDisposable;
        private INotifyPropertyChanged? _framePropertyChanged;
        private IFrame? _frame;
        private SelectionAdorner? _selectionAdorner;
        private SelectedAdorner? _selectedAdorner;
        private bool _dragSelectedItems;
        private Point _start;
        private Rect _selectedRect;
        private Control? _fixedInputSource;

        public Control? InputSource
        {
            get => GetValue(InputSourceProperty);
            set => SetValue(InputSourceProperty, value);
        }

        public Canvas? AdornerCanvas
        {
            get => GetValue(AdornerCanvasProperty);
            set => SetValue(AdornerCanvasProperty, value);
        }

        public bool EnableSnap
        {
            get => GetValue(EnableSnapProperty);
            set => SetValue(EnableSnapProperty, value);
        }

        public double SnapX
        {
            get => GetValue(SnapXProperty);
            set => SetValue(SnapXProperty, value);
        }

        public double SnapY
        {
            get => GetValue(SnapYProperty);
            set => SetValue(SnapYProperty, value);
        }

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == InputSourceProperty)
            {
                DeInitialize();

                if (AssociatedObject is { } && InputSource is not null)
                {
                    Initialize();
                }
            }
        }

        private void Initialize()
        {
            if (AssociatedObject is null || InputSource is null)
            {
                return;
            }

            _fixedInputSource = InputSource;

            _fixedInputSource.AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
            _fixedInputSource.AddHandler(InputElement.PointerReleasedEvent, Released, RoutingStrategies.Tunnel);
            _fixedInputSource.AddHandler(InputElement.PointerCaptureLostEvent, CaptureLost, RoutingStrategies.Tunnel);
            _fixedInputSource.AddHandler(InputElement.PointerMovedEvent, Moved, RoutingStrategies.Tunnel);

            // сброс выделения при запрете редактирования
            _isEditModeDisposable = AssociatedObject.GetObservable(FrameControl.IsEditModeProperty)
                .Subscribe(x =>
                {
                    if (x == false)
                    {
                        RemoveSelection();
                        RemoveSelected();
                    }
                });

            // сброс выделения при смене контекста у элемента управления
            _dataContextDisposable = AssociatedObject
                .GetObservable(StyledElement.DataContextProperty)
                .Subscribe(x =>
                {
                    if (x is IFrame frame)
                    {
                        if (_frame == frame)
                        {
                            if (_framePropertyChanged != null)
                            {
                                _framePropertyChanged.PropertyChanged -= Frame_PropertyChanged;
                            }
                        }

                        RemoveSelection();
                        RemoveSelected();

                        _frame = frame;

                        if (_frame is INotifyPropertyChanged notifyPropertyChanged)
                        {
                            _framePropertyChanged = notifyPropertyChanged;
                            _framePropertyChanged.PropertyChanged += Frame_PropertyChanged;
                        }
                    }
                    else
                    {
                        RemoveSelection();
                        RemoveSelected();
                    }
                });
        }

        private void DeInitialize()
        {
            if (_fixedInputSource is not null)
            {
                _fixedInputSource.RemoveHandler(InputElement.PointerPressedEvent, Pressed);
                _fixedInputSource.RemoveHandler(InputElement.PointerReleasedEvent, Released);
                _fixedInputSource.RemoveHandler(InputElement.PointerCaptureLostEvent, CaptureLost);
                _fixedInputSource.RemoveHandler(InputElement.PointerMovedEvent, Moved);
                _fixedInputSource = null;
            }

            if (_framePropertyChanged is not null)
            {
                _framePropertyChanged.PropertyChanged -= Frame_PropertyChanged;
                _framePropertyChanged = null;
            }

            _isEditModeDisposable?.Dispose();
            _isEditModeDisposable = null;

            _dataContextDisposable?.Dispose();
            _dataContextDisposable = null;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is { } && InputSource is not null)
            {
                Initialize();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            DeInitialize();
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
                        _selectedRect = HitTestHelper.CalculateSelectedRect(AssociatedObject);

                        if (_selectedAdorner is not null)
                        {
                            RemoveSelected();
                        }

                        if (!_selectedRect.IsEmpty && _selectedAdorner is null)
                        {
                            AddSelected(_selectedRect);
                        }
                    }
                    else
                    {
                        RemoveSelected();
                    }
                }
            }
        }

        private void Pressed(object? sender, PointerPressedEventArgs e)
        {
            // при клике на элемент должен установиться фокус контейнера,
            // т.к. после этого взаимодействия должны сразу же начать работать KeyBinding,
            // а управление могло перейти из другого инструмента
            AssociatedObject?.Focus();

            var info = e.GetCurrentPoint(_fixedInputSource);

            if (AssociatedObject?.DataContext is not IFrame drawingNode)
            {
                return;
            }

            if (e.Source is Control { DataContext: IPin })
            {
                return;
            }

            if (!AssociatedObject.GetValue(FrameControl.IsEditModeProperty))
            {
                return;
            }

            var position = e.GetPosition(AssociatedObject);

            if (!drawingNode.CanSelectNodes() && !drawingNode.CanSelectConnectors())
            {
                return;
            }

            if (!info.Properties.IsLeftButtonPressed)
            {
                return;
            }

            _dragSelectedItems = false;

            var pointerHitTestRect = new Rect(position.X - 1, position.Y - 1, 3, 3);

            if (drawingNode.SelectedNodes is { Count: > 0 } || drawingNode.SelectedConnectors is { Count: > 0 })
            {
                if (_selectedRect.Contains(position))
                {
                    _dragSelectedItems = true;
                    _start = SnapHelper.Snap(position, SnapX, SnapY, EnableSnap);
                }
                else
                {
                    HitTestHelper.FindSelectedNodes(AssociatedObject, pointerHitTestRect);

                    if (drawingNode.SelectedNodes is { Count: > 0 } || drawingNode.SelectedConnectors is { Count: > 0 })
                    {
                        _dragSelectedItems = true;
                        _start = SnapHelper.Snap(position, SnapX, SnapY, EnableSnap);
                    }
                    else
                    {
                        drawingNode.SelectedNodes = null;
                        drawingNode.SelectedConnectors = null;
                        RemoveSelected();
                    }
                }
            }
            else
            {
                HitTestHelper.FindSelectedNodes(AssociatedObject, pointerHitTestRect);

                if (drawingNode.SelectedNodes is { Count: > 0 } || drawingNode.SelectedConnectors is { Count: > 0 })
                {
                    _dragSelectedItems = true;
                    _start = SnapHelper.Snap(position, SnapX, SnapY, EnableSnap);
                }
            }

            if (!_dragSelectedItems)
            {
                drawingNode.SelectedNodes = null;
                drawingNode.SelectedConnectors = null;
                RemoveSelected();

                if (e.Source is not Control { DataContext: not IFrame })
                {
                    AddSelection(position.X, position.Y);
                }
            }

            e.Pointer.Capture(_fixedInputSource);

            e.Handled = true;
        }

        private void Released(object? sender, PointerReleasedEventArgs e)
        {
            if (Equals(e.Pointer.Captured, _fixedInputSource))
            {
                if (e.InitialPressMouseButton == MouseButton.Left && AssociatedObject?.DataContext is IFrame)
                {
                    _dragSelectedItems = false;

                    if (e.Source is not Control { DataContext: not IFrame })
                    {
                        if (_selectionAdorner is not null)
                        {
                            HitTestHelper.FindSelectedNodes(AssociatedObject, _selectionAdorner.GetRect());
                        }

                        RemoveSelection();
                    }
                }

                e.Pointer.Capture(null);
            }
        }

        private void CaptureLost(object? sender, PointerCaptureLostEventArgs e)
        {
            RemoveSelection();
        }

        private void Moved(object? sender, PointerEventArgs e)
        {
            var info = e.GetCurrentPoint(_fixedInputSource);

            if (Equals(e.Pointer.Captured, _fixedInputSource) && info.Properties.IsLeftButtonPressed && AssociatedObject?.DataContext is IFrame)
            {
                var position = e.GetPosition(AssociatedObject);

                if (_dragSelectedItems)
                {
                    if (AssociatedObject?.DataContext is IFrame drawingNode)
                    {
                        if (drawingNode.SelectedNodes is { Count: > 0 } && drawingNode.Nodes is { Count: > 0 })
                        {
                            position = SnapHelper.Snap(position, SnapX, SnapY, EnableSnap);

                            var deltaX = position.X - _start.X;
                            var deltaY = position.Y - _start.Y;
                            _start = position;

                            foreach (var node in drawingNode.SelectedNodes)
                            {
                                if (node.CanMove())
                                {
                                    node.Move(deltaX, deltaY);
                                }
                            }

                            var selectedRect = HitTestHelper.CalculateSelectedRect(AssociatedObject);

                            _selectedRect = selectedRect;

                            UpdateSelected(selectedRect);

                            e.Handled = true;
                        }
                    }
                }
                else
                {
                    if (e.Source is not Control { DataContext: not IFrame })
                    {
                        UpdateSelection(position.X, position.Y);

                        e.Handled = true;
                    }
                }
            }
        }

        private void AddSelection(double x, double y)
        {
            var layer = AdornerCanvas;
            if (layer is null)
            {
                return;
            }

            _selectionAdorner = new SelectionAdorner
            {
                IsHitTestVisible = false,
                TopLeft = new Point(x, y),
                BottomRight = new Point(x, y)
            };

            layer.Children.Add(_selectionAdorner);

            InputSource?.InvalidateVisual();
        }

        private void RemoveSelection()
        {
            var layer = AdornerCanvas;
            if (layer is null || _selectionAdorner is null)
            {
                return;
            }

            layer.Children.Remove(_selectionAdorner);
            _selectionAdorner = null;
        }

        private void UpdateSelection(double x, double y)
        {
            var layer = AdornerCanvas;
            if (layer is null)
            {
                return;
            }

            if (_selectionAdorner is { } selection)
            {
                selection.BottomRight = new Point(x, y);
            }

            layer.InvalidateVisual();
            InputSource?.InvalidateVisual();
        }

        private void AddSelected(Rect rect)
        {
            var layer = AdornerCanvas;
            if (layer is null)
            {
                return;
            }

            _selectedAdorner = new SelectedAdorner
            {
                IsHitTestVisible = true,
                Rect = rect
            };

            layer.Children.Add(_selectedAdorner);

            layer.InvalidateVisual();
            InputSource?.InvalidateVisual();
        }

        private void RemoveSelected()
        {
            var layer = AdornerCanvas;
            if (layer is null || _selectedAdorner is null)
            {
                return;
            }

            layer.Children.Remove(_selectedAdorner);
            _selectedAdorner = null;
        }

        private void UpdateSelected(Rect rect)
        {
            var layer = AdornerCanvas;
            if (layer is null)
            {
                return;
            }

            if (_selectedAdorner is { } selected)
            {
                selected.Rect = rect;
            }

            layer.InvalidateVisual();
            InputSource?.InvalidateVisual();
        }
    }
}