using Autonoma.Core;
using System.Collections.Generic;

namespace Autonoma.Domain.Entities
{
    public class AdapterConfiguration : Entity
    {
        public virtual AdapterType AdapterType { get; set; } = new AdapterType();

        public int AdapterTypeId { get; set; } = Globals.IdleAdapterTypeId;

        public string? Address { get; set; }

        public List<DataPointConfiguration> DataPoints { get; set; }

        public string? IpAddress { get; set; }

        public string? Name { get; set; }

        public string? Configuration { get; set; }

        public int Port { get; set; }

        public AdapterConfiguration()
        {
            DataPoints = new List<DataPointConfiguration>();
        }
    }
}
