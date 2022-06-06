using System.Collections.Generic;
using System.Windows.Input;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IFrame : IElement
    {
        ICommand CutCommand { get; }
        ICommand CopyCommand { get; }
        ICommand PasteCommand { get; }
        ICommand SelectAllCommand { get; }
        ICommand DeselectAllCommand { get; }
        ICommand DeleteCommand { get; }

        IList<IElement>? Nodes { get; set; }
        IList<IConnector>? Connectors { get; set; }
        ISet<IElement>? SelectedNodes { get; set; }
        ISet<IConnector>? SelectedConnectors { get; set; }

        bool IsPinConnected(IPin pin);
        bool IsConnectorMoving();
        void CancelConnector();
        bool CanSelectNodes();
        bool CanSelectConnectors();
        bool CanConnectPin(IPin pin);
        void FrameLeftPressed(double x, double y);
        void FrameRightPressed(double x, double y);
        void ConnectorLeftPressed(IPin pin, bool showWhenMoving);
        void ConnectorMove(double x, double y);

        void CutNodes();
        void CopyNodes();
        void PasteNodes();
        void DeleteNodes();
        public IElement Clone(IElement source);

        void SelectAllNodes();
        void DeselectAllNodes();
    }
}
