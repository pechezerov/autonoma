using System.Collections.Generic;

namespace Autonoma.Extensibility.Shared.Contracts
{
    public class PluginRequest
    {
        // Настройки расширения
        public string Plugin { get; set; }
        public List<PluginAttributeInfo> PluginArgs { get; set; }
    }
}
