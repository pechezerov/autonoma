using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.ModelElement
{
    public class ModelElementConfigurationByIdQueryResult : IQueryResult
    {
        public ModelElementConfigurationItem ModelElementTemplate { get; set; } = null!;
    }
}