using Autonoma.API.Queries;
using Autonoma.Domain;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointUpdateQuery : Query
    {
        public DataPointUpdateQuery(int dataPointId, DataValue data)
        {
            DataPointId = dataPointId;
            Data = data;
        }

        public DataValue Data { get; }

        public int DataPointId { get; }
    }
}