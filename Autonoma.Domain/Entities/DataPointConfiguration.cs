using System;

namespace Autonoma.Domain.Entities
{
    public class DataPointConfiguration : Entity
    {
        public virtual AdapterConfiguration Adapter { get; set; } = null!;

        public int AdapterId { get; set; }

        public string Settings { get; set; } = "";

        public string Name { get; set; } = "";

        public DataSource Source { get; set; }

        public TypeCode Type { get; set; } = TypeCode.Int32;

        public string Unit { get; set; } = "";

        public DataPointConfiguration()
        {
        }
    }
}