using Autonoma.App.Services.DataPoint;
using Autonoma.Communication.Abstractions;
using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting.Local
{
    /// <summary>
    /// In-memory datapoint key-value storage
    /// </summary>
    public class DataPointHostService : IDataPointService
    {
        private readonly IRouterService _routerService;
        private ConcurrentDictionary<int, IDataPoint> pointsById = new ConcurrentDictionary<int, IDataPoint>();

        public DataPointHostService(IRouterService routerService)
        {
            _routerService = routerService;
        }

        public Task<DataPointInfo?> GetDataPointValue(int id)
        {
            return Task.Run(() => GetDataPointValueInternal(id));
        }

        private DataPointInfo? GetDataPointValueInternal(int id)
        {
            DataPointInfo? result = null;
            if (pointsById.TryGetValue(id, out var v))
                result = new DataPointInfo(v);

            return result;
        }

        public Task<List<DataPointInfo>> GetDataPointValues(IEnumerable<int>? ids)
        {
            if (ids != null)
            {
                return Task.Run(() => ids
                    .Select(id => GetDataPointValueInternal(id))
                    .Where(dpi => dpi != null)
                    .Cast<DataPointInfo>()
                    .ToList());
            }
            else
            {
                return Task.Run(() => pointsById.Values
                    .Select(v => new DataPointInfo(v))
                    .ToList());
            }
        }

        public void Initialize(List<DataPointConfiguration> pointConfigs)
        {
            foreach (var pointConfig in pointConfigs)
            {
                if (pointsById.ContainsKey(pointConfig.Id))
                {
                    // TODO: report about problem
                    return;
                }

                var point = new SimpleDataPoint(pointConfig);
                pointsById.TryAdd(point.Id, point);
            }
        }

        public void CreateSystemDataPoint(DataPointConfiguration config)
        {
            if (!pointsById.ContainsKey(config.Id))
                pointsById.TryAdd(config.Id, new SimpleDataPoint(config));
        }

        public void RemoveDataPoint(int id)
        {
            IDataPoint? target = null;
            if (pointsById.TryGetValue(id, out target))
                pointsById.TryRemove(target.Id, out var r);
        }

        public async Task UpdateDataPoint(int id, DataValue value)
        {
            IDataPoint? target = null;
            if (pointsById.TryGetValue(id, out target))
                target.Update(value);

            await _routerService.Enqueue(new DataPointInfo(id, value));
        }

        public async Task UpdateDataPoints(IEnumerable<(int id, DataValue value)> updates)
        {
            foreach (var update in updates)
                await UpdateDataPoint(update.id, update.value);
        }
    }
}
