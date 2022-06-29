using Autonoma.Domain.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System;

namespace Autonoma.UI.Presentation.Design
{
    public class DesignEditedViewModel : ViewModelBase
    {
        public bool Bool1 { get; set; }
        public byte Byte1 { get; set; }
        public int Int1 { get; set; }
        public TypeCode Enum1 { get; set; }
        public double Double1 { get; set; }
        public float Float1 { get; set; }
        public string String1 { get; set; } = "";
    }
}
