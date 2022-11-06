using Autonoma.Domain.Abstractions;
using System;

namespace Autonoma.Domain
{
    public class DataPointInfo
    {
        public int AdapterId { get; set; }

        public int DataPointId { get; set; }

        public string? Mapping { get; set; }

        public string? Name { get; set; }

        public DataSource Source { get; set; }

        public TypeCode Type { get; set; }

        public string? Unit { get; set; }

        public DataValue Value { get; set; }

        public DataPointInfo()
        {
            Value = new DataValue();
        }

        public DataPointInfo(IDataPoint v)
        {
            DataPointId = v.Id;
            Name = v.Name;
            Source = v.Source;
            Unit = v.Unit;
            Type = v.Type;
            Value = v.Current;
        }

        public DataPointInfo(int id, DataValue value)
        {
            DataPointId = id;
            Value = value;
        }
    }
}
