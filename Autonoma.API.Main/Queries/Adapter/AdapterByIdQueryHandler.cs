using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.Adapter
{
    public class AdapterByIdQueryHandler : QueryContextlessHandlerAsync<AdapterByIdQuery, AdapterByIdQueryResult>
    {
        public AdapterByIdQueryHandler()
        {
        }

        public override async Task<AdapterByIdQueryResult> ExecuteAsync(AdapterByIdQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
