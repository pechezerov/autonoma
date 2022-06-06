using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using System;

namespace Autonoma.App.Services.DataPoint
{
    public class SimpleDataPoint : IDataPoint
    {
        public SimpleDataPoint(DataPointConfiguration pointConfig)
        {
            Model = pointConfig;
            Current = new DataValue();
        }

        /// <summary>
        /// Current value
        /// </summary>
        public DataValue Current { get; private set; }

        /// <summary>
        /// Configuration prototype
        /// </summary>
        public DataPointConfiguration Model { get; private set; }

        public int Id => Model.Id;
        public string Name => Model.Name;
        public TypeCode Type => Model.Type;
        public string Unit => Model.Unit;
        public DataSource Source => Model.Source;

        /// <summary>
        /// Update counter. Increments when current value changes
        /// </summary>
        public int SequenceNumber { get; private set; }

        public void Update(DataValue value)
        {
            // TODO: type conversion

            if (!Equals(Current, value))
            {
                Current = new DataValue(value);
                SequenceNumber++;
            }
        }
    }
}
