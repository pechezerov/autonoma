using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Extensibility.Shared.Abstractions
{
    /// <summary>
    /// Маркерный интерфейс подключаемых компонентов
    /// </summary>
    public interface IPlugin
    {

    }

    /// <summary>
    /// Маркерный интерфейс коммуникационных плагинов
    /// </summary>
    public interface IAdapterPlugin : IPlugin
    {
        Task StartAsync(CancellationToken token);
    }
}