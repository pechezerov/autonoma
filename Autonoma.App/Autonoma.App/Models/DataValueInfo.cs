using Autonoma.Domain;
using System;

namespace Autonoma.App.Models
{
    public class DataValueInfo
    {
        public static DataValueInfo NoData => new DataValueInfo { };
        public DataQuality Quality { get; set; }
        public DataSource Source { get; set; }
        public DateTime Timestamp { get; set; }
        public Type Type { get; set; }
        public string Value { get; set; }
    }
}
