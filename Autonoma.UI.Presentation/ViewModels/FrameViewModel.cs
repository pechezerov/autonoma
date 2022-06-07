using Autonoma.UI.Presentation.Abstractions;
using Autonoma.UI.Presentation.Infrastructure;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Autonoma.UI.Presentation.ViewModels
{
    public partial class FrameViewModel : ElementViewModel, IFrame
    {
        private string? _clipboard;
        private double _pressedX = double.NaN;
        private double _pressedY = double.NaN;
        private IConnector? _connector;
        private ISet<IElement>? _selectedNodes;
        private IFrameSerializer? _serializer;

        #region Commands

        [JsonIgnore]
        public ICommand SelectAllCommand { get; }

        [JsonIgnore]
        public ICommand CopyCommand { get; }

        [JsonIgnore]
        public ICommand PasteCommand { get; }

        [JsonIgnore]
        public ICommand CutCommand { get; }

        [JsonIgnore]
        public ICommand DeleteCommand { get; }

        [JsonIgnore]
        public ICommand DeselectAllCommand { get; }

        #endregion

        /// <summary>
        /// Все элементы, входящие в состав кадра
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<IElement> Nodes { get; set; }

        [Browsable(false)]
        IList<IElement>? IFrame.Nodes
        {
            get => Nodes; set
            {
                Nodes.Clear();
                if (value != null)
                {
                    foreach (var item in value)
                        Nodes.Add(item);
                }
            }
        }

        /// <summary>
        /// Все коннекторы, входящие в состав кадра
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<IConnector> Connectors { get; set; }

        [Browsable(false)]
        IList<IConnector>? IFrame.Connectors
        {
            get => Connectors; set
            {
                Connectors.Clear();
                if (value != null)
                {
                    foreach (var item in value)
                        Connectors.Add(item);
                }
            }
        }

        /// <summary>
        /// Коннекторы, выбранные для редактирования
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        [Reactive]
        public ISet<IConnector>? SelectedConnectors { get; set; }

        /// <summary>
        /// Узлы, выбранные для редактирования
        /// </summary>
        [JsonIgnore]
        [Browsable(false)]
        public ISet<IElement>? SelectedNodes
        {
            get => _selectedNodes;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedNodes, value);
                SelectedNode = _selectedNodes?.FirstOrDefault();
            }
        }

        /// <summary>
        /// Узел, доступный для редактирования
        /// </summary>
        /// <remarks>
        /// Если выбрано несколько узлов, доступным считается лишь один
        /// </remarks>
        [JsonIgnore]
        [Browsable(false)]
        [Reactive]
        public IElement? SelectedNode { get; set; }

        public FrameViewModel(IFrameSerializer? serializer)
        {
            _serializer = serializer;
            Connectors = new ObservableCollection<IConnector>();
            Nodes = new ObservableCollection<IElement>();

            CutCommand = ReactiveCommand.Create(CutNodes);
            CopyCommand = ReactiveCommand.Create(CopyNodes);
            PasteCommand = ReactiveCommand.Create(PasteNodes);
            SelectAllCommand = ReactiveCommand.Create(SelectAllNodes);
            DeselectAllCommand = ReactiveCommand.Create(DeselectAllNodes);
            DeleteCommand = ReactiveCommand.Create(DeleteNodes);
        }

        #region Connectors

        public void CancelConnector()
        {
            if (_connector is not null)
            {
                if (Connectors is not null)
                {
                    Connectors.Remove(_connector);
                }

                _connector = null;
            }
        }

        public virtual bool CanConnectPin(IPin pin)
        {
            return true;
        }

        public void ConnectorLeftPressed(IPin pin, bool showWhenMoving)
        {
            if (Connectors is null)
            {
                return;
            }

            if (!CanConnectPin(pin) || !pin.CanConnect())
            {
                return;
            }

            if (_connector is null)
            {
                var x = pin.X;
                var y = pin.Y;

                if (pin.Parent is not null)
                {
                    x += pin.Parent.X;
                    y += pin.Parent.Y;
                }

                var end = new PinViewModel
                {
                    Parent = null,
                    X = x,
                    Y = y,
                    Width = pin.Width,
                    Height = pin.Height
                };

                var connector = new ConnectorViewModel
                {
                    Parent = this,
                    Start = pin,
                    End = end
                };

                if (showWhenMoving)
                {
                    Connectors ??= new ObservableCollection<IConnector>();
                    Connectors.Add(connector);
                }

                _connector = connector;
            }
            else
            {
                if (_connector.Start != pin)
                {
                    _connector.End = pin;

                    if (!showWhenMoving)
                    {
                        Connectors ??= new ObservableCollection<IConnector>();
                        Connectors.Add(_connector);
                    }

                    _connector = null;
                }
            }
        }

        public virtual void ConnectorMove(double x, double y)
        {
            if (_connector is { End: { } })
            {
                _connector.End.X = x;
                _connector.End.Y = y;
            }
        }

        public bool IsConnectorMoving()
        {
            if (_connector is not null)
                return true;

            return false;
        }

        public bool IsPinConnected(IPin pin)
        {
            if (Connectors is not null)
            {
                foreach (var connector in Connectors)
                {
                    if (connector.Start == pin || connector.End == pin)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Selection

        public virtual bool CanSelectConnectors()
        {
            if (_connector is not null)
            {
                return false;
            }

            return true;
        }

        public virtual bool CanSelectNodes()
        {
            if (_connector is not null)
            {
                return false;
            }

            return true;
        }

        public virtual void DeselectAllNodes()
        {
            SelectedNodes = null;
            SelectedConnectors = null;

            if (IsConnectorMoving())
            {
                CancelConnector();
            }
        }

        public virtual void SelectAllNodes()
        {
            if (Nodes is not null)
            {
                SelectedNodes = null;

                var selectedNodes = new HashSet<IElement>();
                var nodes = Nodes;

                foreach (var node in nodes)
                {
                    if (node.CanSelect())
                    {
                        selectedNodes.Add(node);
                    }
                }

                if (selectedNodes.Count > 0)
                {
                    SelectedNodes = selectedNodes;
                }
            }

            if (Connectors is not null)
            {
                SelectedConnectors = null;

                var selectedConnectors = new HashSet<IConnector>();
                var connectors = Connectors;

                foreach (var connector in connectors)
                {
                    if (connector.CanSelect())
                    {
                        selectedConnectors.Add(connector);
                    }
                }

                if (selectedConnectors.Count > 0)
                {
                    SelectedConnectors = selectedConnectors;
                }
            }
        }

        #endregion

        #region Cut/Copy/Paste

        public IElement Clone(IElement source)
        {
            if (_serializer == null)
                throw new InvalidOperationException();

            var cloneData = _serializer.Serialize(source);
            var clonedElement = _serializer.DeserializeElement(cloneData);
            return clonedElement;
        }

        public void CopyNodes()
        {
            if (_serializer is null)
                return;

            if (SelectedNodes is not { Count: > 0 } && SelectedConnectors is not { Count: > 0 })
                return;

            var clipboard = new FrameClipboard
            {
                SelectedNodes = SelectedNodes,
                SelectedConnectors = SelectedConnectors
            };

            _clipboard = _serializer.Serialize(clipboard);
        }

        public void CutNodes()
        {
            if (_serializer is null)
                return;

            if (SelectedNodes is not { Count: > 0 } && SelectedConnectors is not { Count: > 0 })
                return;

            var clipboard = new FrameClipboard
            {
                SelectedNodes = SelectedNodes,
                SelectedConnectors = SelectedConnectors
            };

            _clipboard = _serializer.Serialize(clipboard);

            if (clipboard.SelectedNodes is not null)
            {
                foreach (var node in clipboard.SelectedNodes)
                {
                    if (node.CanRemove())
                    {
                        Nodes?.Remove(node);
                    }
                }
            }

            if (clipboard.SelectedConnectors is not null)
            {
                foreach (var connector in clipboard.SelectedConnectors)
                {
                    if (connector.CanRemove())
                    {
                        Connectors?.Remove(connector);
                    }
                }
            }

            SelectedNodes = null;
            SelectedConnectors = null;
        }

        public void DeleteNodes()
        {
            if (SelectedNodes is { Count: > 0 })
            {
                var selectedNodes = SelectedNodes;

                foreach (var node in selectedNodes)
                {
                    if (node.CanRemove())
                    {
                        Nodes?.Remove(node);
                    }
                }

                SelectedNodes = null;
            }

            if (SelectedConnectors is { Count: > 0 })
            {
                var selectedConnectors = SelectedConnectors;

                foreach (var connector in selectedConnectors)
                {
                    if (connector.CanRemove())
                    {
                        Connectors?.Remove(connector);
                    }
                }

                SelectedConnectors = null;
            }
        }

        public void PasteNodes()
        {
            if (_serializer is null)
                return;

            if (_clipboard is null)
                return;

            var pressedX = _pressedX;
            var pressedY = _pressedY;

            var clipboard = _serializer.Deserialize<FrameClipboard?>(_clipboard);

            if (clipboard is null)
                return;

            SelectedNodes = null;
            SelectedConnectors = null;

            var selectedNodes = new HashSet<IElement>();
            var selectedConnectors = new HashSet<IConnector>();

            if (clipboard.SelectedNodes is { Count: > 0 })
            {
                var minX = 0.0;
                var minY = 0.0;
                var i = 0;

                foreach (var node in clipboard.SelectedNodes)
                {
                    minX = i == 0 ? node.X : Math.Min(minX, node.X);
                    minY = i == 0 ? node.Y : Math.Min(minY, node.Y);
                    i++;
                }

                var deltaX = double.IsNaN(pressedX) ? 0.0 : pressedX - minX;
                var deltaY = double.IsNaN(pressedY) ? 0.0 : pressedY - minY;

                foreach (var node in clipboard.SelectedNodes)
                {
                    if (node.CanMove())
                    {
                        node.Move(deltaX, deltaY);
                    }

                    node.Parent = this;

                    Nodes?.Add(node);

                    if (node.CanSelect())
                    {
                        selectedNodes.Add(node);
                    }
                }
            }

            if (clipboard.SelectedConnectors is { Count: > 0 })
            {
                foreach (var connector in clipboard.SelectedConnectors)
                {
                    connector.Parent = this;

                    Connectors?.Add(connector);

                    if (connector.CanSelect())
                    {
                        selectedConnectors.Add(connector);
                    }
                }
            }

            if (selectedNodes.Count > 0)
            {
                SelectedNodes = selectedNodes;
            }

            if (selectedConnectors.Count > 0)
            {
                SelectedConnectors = selectedConnectors;
            }

            _pressedX = double.NaN;
            _pressedY = double.NaN;
        }

        #endregion 

        public void FrameLeftPressed(double x, double y)
        {
        }

        public void FrameRightPressed(double x, double y)
        {
        }
    }
}
