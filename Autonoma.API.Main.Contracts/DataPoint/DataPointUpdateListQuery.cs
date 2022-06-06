using Autonoma.API.Queries;
using Autonoma.Domain;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointUpdateListQuery : Query
    {
        public DataPointUpdateListQuery(List<(int, DataValue)> data)
        {
            Data = data;
        }

        public List<(int dataPointId, DataValue data)> Data { get; }
    }
}