using Autonoma.Domain;
using System;

namespace Autonoma.API.Main.Contracts.DataPoint
{
    public class DataPointConfigurationItem
    {
        public int AdapterId { get; set; }
        public int Id { get; set; }
        public string Mapping { get; set; }
        public string Name { get; set; }
        public DataSource Source { get; set; }
        public TypeCode Type { get; set; }
        public string Unit { get; set; }
    }
}
