using System.Collections.Generic;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IElementPrototypesHost
    {
        IList<IElementPrototype> Prototypes { get; set; }
    }
}