using PrettyScreen.Configuration;
using System;

namespace PrettyScreen.Core
{
    public class SimpleDataPoint : IDataPoint
    {
        public SimpleDataPoint(DataPointConfiguration pointModel)
        {
            Model = pointModel;
            Current = new DataValue();
        }

        public DataValue Current { get; private set; }
        public DataPointConfiguration Model { get; private set; }

        public Guid Id => Model.Id;
        public string Name => Model.Name;
        public TypeCode Type => Model.Type;
        public string Unit => Model.Unit;
        public DataSource Source => Model.Source;

        public void Update(DataValue value)
        {
            // TODO: type conversion
            if (!object.Equals(Current, value))
                Current = new DataValue(value);
        }
    }
}
