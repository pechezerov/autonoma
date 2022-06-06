using Autonoma.API.Main.Contracts.Common;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointByIdQuery : EntityByIdQuery
    {
        public DataPointByIdQuery(int id) : base(id)
        {
            Id = id;
        }
    }
}