using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterConfigurationByIdQuery : EntityByIdQuery
    {
        public AdapterConfigurationByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}