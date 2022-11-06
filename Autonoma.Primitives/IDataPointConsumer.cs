using Autonoma.Domain;

namespace Autonoma.Primitives
{
    public interface IDataPointConsumer
    {
        void Update(DataPointInfo? data);
    }
}
