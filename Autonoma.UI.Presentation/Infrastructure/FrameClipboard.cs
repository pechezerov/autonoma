using Autonoma.UI.Presentation.Abstractions;
using System.Collections.Generic;

namespace Autonoma.UI.Presentation.Infrastructure
{
    /// <summary>
    /// Примитив упаковки данных при переносе элементов кадра
    /// </summary>
    public class FrameClipboard
    {
        public ISet<IConnector>? SelectedConnectors { get; set; }
        public ISet<IElement>? SelectedNodes { get; set; }
    }
}
