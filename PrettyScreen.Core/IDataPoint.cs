using System;

namespace PrettyScreen.Core
{
    public interface IDataPoint : IUnique
    {
        DataValue Current { get; }
        string Name { get; }
        TypeCode Type { get; }
        string Unit { get; }
        DataSource Source { get; }

        void Update(DataValue value);
    }
}