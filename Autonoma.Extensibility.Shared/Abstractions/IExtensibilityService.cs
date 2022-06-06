using Autonoma.Extensibility.Shared.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.Extensibility.Shared.Abstractions
{
    public interface IExtensibilityService
    {
        Task Reload();
        List<PluginInfo> GetImplementations<T>();
        T GetInstance<T>(string pluginName);
    }
}
