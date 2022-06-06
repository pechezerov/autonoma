using Autonoma.API.Main.Contracts.Common;
using System;

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