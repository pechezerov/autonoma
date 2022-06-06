using Autonoma.UI.Presentation.Abstractions;

namespace Autonoma.UI.Presentation.Infrastructure
{
    public class ElementPrototype : IElementPrototype
    {
        public string? Title { get; set; }
        public string? AssemblyQualifiedTypeName { get; set; }
        public IElement? Template { get; set; }
        public IElement? Preview { get; set; }
    }
}
