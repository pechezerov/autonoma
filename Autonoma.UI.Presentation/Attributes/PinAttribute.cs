using Autonoma.UI.Presentation.Model;
using System;

namespace Autonoma.UI.Presentation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PinAttribute : Attribute
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public PinAlignment Alignment { get; set; }
        public PinDirection Direction { get; set; }
        public string Name { get; set; }

        public PinAttribute(double x, double y, PinAlignment alignment = PinAlignment.None, PinDirection direction = PinDirection.None, string? name = null)
            : this(x, y, 4, 4, alignment, direction, name) { }

        public PinAttribute(double x, double y, double width, double height, PinAlignment alignment, PinDirection direction, string? name)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Alignment = alignment;
            Direction = direction;
            Name = name ?? "";
        }
    }
}
