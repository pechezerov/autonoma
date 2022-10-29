using Autonoma.API.Dtos;
using Autonoma.API.Queries;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.ModelElementTemplate
{
    public class ModelElementTemplateConfigurationListQueryResult : PaginatedItemsList<ModelElementTemplateConfigurationItem>, IQueryResult
    {
        public ModelElementTemplateConfigurationListQueryResult(int pageIndex, int pageSize, long count, IEnumerable<ModelElementTemplateConfigurationItem> data)
            : base(pageIndex, pageSize, count, data)
        { }
    }
}
