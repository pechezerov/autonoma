using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using System;

namespace Autonoma.UI.Presentation.Design
{
    public class DesignDataPointViewModel : IDataPoint
    {
        public DataValue Current => DataValue.NoData;

        public string Name => "Test";

        public TypeCode Type => TypeCode.Double;

        public string Unit => "MAut";

        public DataSource Source => DataSource.Device;

        public int Id => -1;

        public void Update(DataValue value) { }
    }
}
