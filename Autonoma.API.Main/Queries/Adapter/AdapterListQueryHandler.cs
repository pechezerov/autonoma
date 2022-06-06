using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Queries;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Queries.Adapter
{
    public class AdapterListQueryHandler : QueryContextlessHandlerAsync<AdapterListQuery, AdapterListQueryResult>
    {
        public AdapterListQueryHandler()
        {
        }

        public override async Task<AdapterListQueryResult> ExecuteAsync(AdapterListQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
