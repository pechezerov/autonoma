using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Autonoma.Domain
{
    public record DataValue
    {
        public DataValue(DataValue value)
        {
            Value = value.Value;
            Timestamp = value.Timestamp;
            Quality = value.Quality;
            Source = value.Source;
        }

        public DataValue()
        {
            Value = null;
            Timestamp = DateTime.Now;
            Quality = DataQuality.Bad;
            Source = DataSource.Device;
        }

        public DataValue(object val, DateTime dt)
        {
            Value = val;
            Timestamp = dt;
            Quality = DataQuality.Ok;
            Source = DataSource.Device;
        }

        public object? Value { get; set; }
        public DateTime Timestamp { get; set; }
        public DataQuality Quality { get; set; }
        public DataSource Source { get; set; }

        [JsonIgnore]
        private Type? ValueType => Value?.GetType();

        public int? ValueTypeCode
        {
            get
            {
                if (Value != null)
                {
                    if (Value is double)
                        return (int)TypeCode.Double;
                    if (Value is bool)
                        return (int)TypeCode.Boolean;
                    if (Value is int)
                        return (int)TypeCode.Int32;
                    if (Value is string)
                        return (int)TypeCode.String;
                    return (int)TypeCode.Object;
                }
                return null;
            }
        }

        public double ValueDouble() => Convert.ToDouble(Value, CultureInfo.CurrentCulture);
        
        public int ValueInt() => Convert.ToInt32(Value, CultureInfo.CurrentCulture);

        public string? ValueString() => Convert.ToString(Value, CultureInfo.CurrentCulture);

        public static DataValue NoData => new DataValue { };
    }
}