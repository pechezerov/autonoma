using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointConfigurationByIdQuery : EntityByIdQuery
    {
        public DataPointConfigurationByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}