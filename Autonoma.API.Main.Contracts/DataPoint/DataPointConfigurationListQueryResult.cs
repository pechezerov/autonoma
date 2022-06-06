using Autonoma.API.Dtos;
using Autonoma.API.Queries;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointConfigurationListQueryResult : PaginatedItemsList<DataPointConfigurationItem>, IQueryResult
    {
        public DataPointConfigurationListQueryResult(int pageIndex, int pageSize, long count, IEnumerable<DataPointConfigurationItem> data)
            : base(pageIndex, pageSize, count, data)
        { }
    }
}
