using Autonoma.UI.Presentation.Model;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IPin
    {
        string? Name { get; set; }
        IElement? Parent { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        PinAlignment Alignment { get; set; }
        PinDirection Direction { get; set; }
        bool CanConnect();
        bool CanDisconnect();
    }
}
