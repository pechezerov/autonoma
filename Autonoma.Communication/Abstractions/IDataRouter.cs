using Autonoma.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Infrastructure
{
    public interface IDataRouter
    {
        Task SendUpdates(List<DataPointInfo> updates);
    }
}