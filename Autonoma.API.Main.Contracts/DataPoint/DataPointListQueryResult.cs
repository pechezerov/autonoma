using Autonoma.API.Queries;
using Autonoma.Domain;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointListQueryResult : IQueryResult
    {
        public List<DataPointInfo> DataPoints { get; set; }
    }
}