using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting
{
    /// <summary>
    /// Интерфейс компонента доступа к действующим адаптерам
    /// </summary>
    public interface IAdapterService : IDisposable
    {
        IEnumerable<IDataAdapter> Adapters { get; }
        Task DeleteAdapter(int id);
        Task UpdateAdapter(AdapterConfiguration adapterConfiguration);
    }
}