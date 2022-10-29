using Autonoma.API.Main.Contracts.DataPoint;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.Adapter
{
    public class AdapterConfigurationItem
    {
        public int Id { get; set; }
        public int AdapterTypeId { get; set; }
        public AdapterTypeItem AdapterType { get; set; } = new AdapterTypeItem();
        public List<DataPointConfigurationItem> DataPoints { get; set; } = new List<DataPointConfigurationItem>();
        public string Name { get; set; } = "";
        public string Settings { get; set; } = "";
    }
}
