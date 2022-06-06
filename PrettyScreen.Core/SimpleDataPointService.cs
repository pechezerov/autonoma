using PrettyScreen.Configuration;
using System;
using System.Collections.Generic;

namespace PrettyScreen.Core
{
    public class SimpleDataPointService : IDataPointService
    {
        private Dictionary<Guid, IDataPoint> pointsById = new Dictionary<Guid, IDataPoint>();

        public string Name { get; set; }

        public SimpleDataPointService(IDataStore<DataPointConfiguration> model)
        {
            foreach (var pointConfig in model.Items)
            {
                CreateOrUpdateDataPoint(pointConfig, false);
            }
        }

        public void CreateOrUpdateDataPoint(DataPointConfiguration pointConfig, bool recreate)
        {
            if (pointsById.ContainsKey(pointConfig.Id))
            {
                if (!recreate)
                {
                    // TODO: report about problem
                    return;
                }
                else
                {
                    pointsById.Remove(pointConfig.Id);
                }
            }

            var point = new SimpleDataPoint(pointConfig);
            pointsById.Add(point.Id, point);
        }

        public IEnumerable<IDataPoint> DataPoints => pointsById.Values;


        public IDataPoint GetDataPoint(Guid id)
        {
            IDataPoint result = null;
            pointsById.TryGetValue(id, out result);
            return result;
        }

        public void UpdateDataPoint(Guid id, DataValue value)
        {
            IDataPoint target = null;
            if (pointsById.TryGetValue(id, out target))
                target.Update(value);
        }


        public void RemoveDataPoint(Guid id)
        {
            IDataPoint target = null;
            if (pointsById.TryGetValue(id, out target))
            {
                pointsById.Remove(target.Id);
            }
        }
    }
}
