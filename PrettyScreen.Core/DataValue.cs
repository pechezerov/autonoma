using System;
using System.Globalization;

namespace PrettyScreen.Core
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
            Source = DataSource.Field;
        }

        public DataValue(object val, DateTime dt)
        {
            Value = val;
            Timestamp = dt;
            Quality = DataQuality.Ok;
            Source = DataSource.Field;
        }

        public object Value { get; set; }
        public DateTime Timestamp { get; set; }
        public DataQuality Quality { get; set; }
        public DataSource Source { get; set; }

        public Type ValueType
        {
            get
            {
                if (Value != null)
                {
                    return Value.GetType();
                }
                return null;
            }
        }
        public double ValueDouble => Convert.ToDouble(Value, CultureInfo.CurrentCulture);
        public int ValueInt => Convert.ToInt32(Value, CultureInfo.CurrentCulture);
        public string ValueString => Convert.ToString(Value, CultureInfo.CurrentCulture);

        public static DataValue NoData => new DataValue { };
    }
}