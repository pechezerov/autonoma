using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.ModelElement
{
    public class ModelElementConfigurationByIdQuery : EntityByIdQuery
    {
        public ModelElementConfigurationByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}