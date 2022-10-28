using AutoMapper;
using Autonoma.API.Main.Client;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Main.Contracts.DataPoint;
using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;

namespace Autonoma.Communication.Hosting.Remote
{
    public class RemoteService : IAdapterService, IDataPointService
    {
        public APIMainClient MainApi { get; }

        public IMapper Mapper { get; }

        public RemoteService(APIMainClient mainApi, IMapper mapper)
        {
            MainApi = mainApi;
            Mapper = mapper;
        }

        #region Adapters

        public IEnumerable<IDataAdapter> Adapters => throw new NotImplementedException();

        public async Task<AdapterConfiguration> GetAdapter(int id)
        {
            var dp = await MainApi.AdaptersConfiguration_AdapterByIdAsync(id);
            return Mapper.Map<AdapterConfiguration>(dp);
        }

        public async Task UpdateAdapter(AdapterConfiguration adapterConfiguration)
        {
            await MainApi.AdaptersConfiguration_UpdateAdapterConfigurationAsync(Mapper.Map<AdapterConfigurationItem>(adapterConfiguration));
        }

        public async Task DeleteAdapter(int id)
        {
            throw new NotImplementedException("Внешние клиенты не могут удалять адаптеры");
        }

        #endregion

        #region DataPoints

        public async Task<IEnumerable<DataPointInfo>> DataPoints()
        {
            var adapters =  (await MainApi.Adapters_AdapterListAsync(new AdapterListQuery())).Data;
            var dataPoints = new List<DataPointInfo>();

            foreach (var adapter in adapters)
            {
                var adapterDataPoints = (await MainApi.DataPoints_DataPointListAsync(
                    new DataPointListQuery { Ids = adapter.Configuration.DataPoints.Select(dp => dp.Id) })).DataPoints
                        .Select(dp => new DataPointInfo(dp.DataPointId, dp.Value));
            }
            return dataPoints;
        }

        public async Task<DataPointInfo?> GetDataPointValue(int id)
        {
            var dp = await MainApi.DataPoints_DataPointByIdAsync(id);
            return dp.DataPoint;
        }

        public async Task<List<DataPointInfo>> GetDataPointValues(IEnumerable<int>? ids)
        {
            if (ids == null)
                return new List<DataPointInfo>();
            var dps = await MainApi.DataPoints_DataPointListAsync(new API.Main.Contracts.DataPoint.DataPointListQuery { Ids = ids });
            return dps.DataPoints;
        }

        public async Task UpdateDataPoint(int id, DataValue value)
        {
            await MainApi.DataPoints_UpdateDataPointAsync(id, value);
        }

        public async Task UpdateDataPoints(IEnumerable<(int id, DataValue value)> updates)
        {
            await MainApi.DataPoints_UpdateDataPointsAsync(
                new API.Main.Contracts.DataPoint.DataPointUpdateListQuery(updates.ToList()));
        }

        public void CreateSystemDataPoint(DataPointConfiguration config)
        {
            throw new NotImplementedException("Внешние клиенты не могут создавать точки данных");
        }

        public void RemoveDataPoint(int id)
        {
            throw new NotImplementedException("Внешние клиенты не могут удалять точки данных");
        }

        #endregion

        public void Dispose()
        {
            return;
        }

        public void Initialize(List<DataPointConfiguration> pointConfigs)
        {
            // initialization done on the hosting-side
        }
    }
}
