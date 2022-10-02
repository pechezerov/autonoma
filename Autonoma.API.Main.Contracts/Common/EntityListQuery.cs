using Autonoma.API.Queries;
using Autonoma.API.Shared.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Autonoma.API.Main.Contracts.Common
{
    public class EntityListQuery : Query
    {
        [DefaultValue(10)]
        public int PageSize { get; set; } = 0;

        [DefaultValue(1)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(BinderType = typeof(CommaSeparatedListBinder<int>))]
        public IEnumerable<int> Ids { get; set; } = Enumerable.Empty<int>();
    }
}