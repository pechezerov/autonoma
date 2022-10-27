using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterByIdQuery : EntityByIdQuery
    {
        public AdapterByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}