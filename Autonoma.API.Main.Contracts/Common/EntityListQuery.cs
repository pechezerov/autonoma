using Autonoma.API.Queries;
using System.ComponentModel;

namespace Autonoma.API.Main.Contracts.Common
{
    public class EntityListQuery : Query
    {
        [DefaultValue(10)]
        public int PageSize { get; set; }

        [DefaultValue(1)]
        public int PageIndex { get; set; }

        public string? Ids { get; set; }
    }
}
