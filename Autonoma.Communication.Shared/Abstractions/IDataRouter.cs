using Autonoma.Domain;

namespace Autonoma.Communication.Abstractions
{
    public interface IDataRouter
    {
        Task SendUpdates(List<DataPointInfo> updates);
    }
}