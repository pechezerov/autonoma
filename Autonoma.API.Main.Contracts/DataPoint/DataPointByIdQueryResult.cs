using Autonoma.API.Queries;
using Autonoma.Domain;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointByIdQueryResult : IQueryResult
    {
        public DataPointInfo DataPoint { get; set; } = null!;
    }
}