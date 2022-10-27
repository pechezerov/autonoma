using System;

namespace Autonoma.UI.Presentation.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DefaultSizeAttribute : Attribute
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public DefaultSizeAttribute(double width, double height)
        {
            Width = width;
            Height = height;
        }
    }
}
