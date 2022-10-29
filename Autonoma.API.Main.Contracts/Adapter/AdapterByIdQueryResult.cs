using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterByIdQueryResult : IQueryResult
    {
        public AdapterItem Adapter { get; set; } = null!;
    }
}