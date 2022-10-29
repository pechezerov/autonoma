using Autonoma.API.Queries;

namespace Autonoma.API.Main.Contracts.ModelElementTemplate
{
    public class ModelElementTemplateConfigurationByIdQueryResult : IQueryResult
    {
        public ModelElementTemplateConfigurationItem ModelElementTemplateConfiguration { get; set; } = null!;
    }
}