using System;
using System.Collections.Generic;

namespace Autonoma.UI.Presentation.Abstractions
{
    public interface IElementFactory
    {
        IList<IElementPrototype> CreateToolbox();

        IElement CreateElement(Type elementContentType);
    }
}
