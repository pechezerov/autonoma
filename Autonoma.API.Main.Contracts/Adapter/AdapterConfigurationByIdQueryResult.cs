using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterConfigurationByIdQueryResult : IQueryResult
    {
        public AdapterConfigurationItem Adapter { get; set; }
    }
}