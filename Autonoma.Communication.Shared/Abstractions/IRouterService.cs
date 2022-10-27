using Autonoma.Domain;

namespace Autonoma.Communication.Abstractions
{
    public interface IRouterService
    {
        Task Enqueue(DataPointInfo item);

        Task Enqueue(IEnumerable<DataPointInfo> items);

        void SubscribeToDataPoints(string connectionId, List<int> dataPointIds);
    }
}