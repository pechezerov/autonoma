using Autonoma.Domain;
using System;

namespace Autonoma.App.Models
{
    public class DataPointInfo
    {
        public int Id { get; set; }
        public int AdapterId { get; set; }
        public string Name { get; set; }
        public TypeCode Type { get; set; }
        public string Unit { get; set; }
        public DataSource Source { get; set; }
        public string Mapping { get; set; }

        public DataValueInfo Value { get; set; }
    }
}