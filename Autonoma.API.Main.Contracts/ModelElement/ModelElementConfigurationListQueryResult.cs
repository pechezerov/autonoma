using Autonoma.API.Dtos;
using Autonoma.API.Queries;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.ModelElement
{
    public class ModelElementConfigurationListQueryResult : PaginatedItemsList<ModelElementConfigurationItem>, IQueryResult
    {
        public ModelElementConfigurationListQueryResult(int pageIndex, int pageSize, long count, IEnumerable<ModelElementConfigurationItem> data)
            : base(pageIndex, pageSize, count, data)
        { }
    }
}
