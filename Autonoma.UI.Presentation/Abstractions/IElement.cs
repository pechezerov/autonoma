using Autonoma.Domain;
using Autonoma.UI.Presentation.Model;
using System.Collections.Generic;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IElement
    {
        string Name { get; set; }
        IElement? Parent { get; set; }
        double X { get; set; }
        double Y { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        object? Content { get; set; }
        IList<IPin>? Pins { get; }

        int? LinkedDataPointId { get; set; }

        bool CanSelect();
        bool CanRemove();
        bool CanMove();
        bool CanResize();
        void Move(double deltaX, double deltaY);
        void Resize(double deltaX, double deltaY, NodeResizeDirection direction);
    }
}
