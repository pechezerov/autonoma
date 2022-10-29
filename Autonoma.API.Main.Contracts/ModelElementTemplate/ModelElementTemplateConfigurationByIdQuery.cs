using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.ModelElementTemplate
{
    public class ModelElementTemplateConfigurationByIdQuery : EntityByIdQuery
    {
        public ModelElementTemplateConfigurationByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}