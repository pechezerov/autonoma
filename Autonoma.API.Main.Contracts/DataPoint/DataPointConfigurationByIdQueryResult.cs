using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointConfigurationByIdQueryResult : IQueryResult
    {
        public DataPointConfigurationItem DataPoint { get; set; }
    }
}