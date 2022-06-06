using Autonoma.API.Dtos;
using Autonoma.API.Queries;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterListQueryResult : PaginatedItemsList<AdapterItem>, IQueryResult
    {
        public AdapterListQueryResult(int pageIndex, int pageSize, long count, IEnumerable<AdapterItem> data)
            : base(pageIndex, pageSize, count, data)
        { }
    }
}
