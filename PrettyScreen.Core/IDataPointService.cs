using PrettyScreen.Configuration;
using System;
using System.Collections.Generic;

namespace PrettyScreen.Core
{

    public interface IDataPointService
    {
        void CreateOrUpdateDataPoint(DataPointConfiguration pointConfig, bool recreate);

        IDataPoint GetDataPoint(Guid id);
        void UpdateDataPoint(Guid id, DataValue value);
        void RemoveDataPoint(Guid id);

        IEnumerable<IDataPoint> DataPoints { get; }
    }
}