using Autonoma.API.Dtos;
using Autonoma.API.Queries;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterConfigurationListQueryResult : PaginatedItemsList<AdapterConfigurationItem>, IQueryResult
    {
        public AdapterConfigurationListQueryResult(int pageIndex, int pageSize, long count, IEnumerable<AdapterConfigurationItem> data)
            : base(pageIndex, pageSize, count, data)
        { }
    }
}
