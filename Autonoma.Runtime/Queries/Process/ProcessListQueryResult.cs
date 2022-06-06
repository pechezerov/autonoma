using System.Collections.Generic;
using System.Linq;

namespace Autonoma.Runtime.Queries.Process
{
    public class ProcessListQueryResult
    {
        public List<ProcessItem> Items { get; set; }

        public ProcessListQueryResult(IEnumerable<ProcessItem> items)
        {
            Items = items.ToList();
        }
    }
}
