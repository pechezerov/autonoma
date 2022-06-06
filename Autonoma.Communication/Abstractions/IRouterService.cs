using Autonoma.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.Communication.Abstractions
{
    public interface IRouterService
    {
        Task Enqueue(DataPointInfo item);

        Task Enqueue(IEnumerable<DataPointInfo> items);

        void SubscribeToDataPoints(string connectionId, List<int> dataPointIds);
    }
}