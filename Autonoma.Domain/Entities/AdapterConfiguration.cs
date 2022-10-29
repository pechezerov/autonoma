using Autonoma.Core;
using System.Collections.Generic;

namespace Autonoma.Domain.Entities
{
    public class AdapterConfiguration : Entity
    {
        /// <summary>
        /// Тип адаптера
        /// </summary>
        public virtual AdapterType AdapterType { get; set; } = new AdapterType();

        public int AdapterTypeId { get; set; } = Globals.IdleAdapterTypeId;

        public string Name { get; set; } = "";

        public string Settings { get; set; } = "";

        public List<DataPointConfiguration> DataPoints { get; set; }

        public AdapterConfiguration()
        {
            DataPoints = new List<DataPointConfiguration>();
        }
    }
}
