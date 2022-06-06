using Autonoma.UI.Presentation.Model;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IConnector
    {
        string? Name { get; set; }
        IFrame? Parent { get; set; }
        ConnectorOrientation Orientation { get; set; }
        ConnectorType Type { get; set; }
        IPin? Start { get; set; }
        IPin? End { get; set; }
        double Offset { get; set; }
        bool CanSelect();
        bool CanRemove();
    }
}
