using Autonoma.API.Main.Contracts.Common;
using System.Collections.Generic;
using System.Linq;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointListQuery : EntityListQuery
    {
        public DataPointListQuery(string ids)
        {
            Ids = ids;
        }

        public List<int>? SplitIntoNumbers() => Ids.Split(',').Select(id => (Ok: int.TryParse(id, out int x), Value: x))
            .Where(x => x.Ok)
            .Select(x => x.Value)
            .ToList();
    }
}