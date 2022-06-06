using AutoMapper;
using Autonoma.API.Main.Client;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting.Remote
{
    public class RemoteService : IAdapterService, IDataPointService
    {
        public IMainApi MainApi { get; }

        public IMapper Mapper { get; }

        public RemoteService(IMainApi mainApi, IMapper mapper)
        {
            MainApi = mainApi;
            Mapper = mapper;
        }

        #region Adapters

        public IEnumerable<IDataAdapter> Adapters => throw new NotImplementedException();

        public AdapterConfiguration GetAdapter(int id)
        {
            var dp = MainApi.AdaptersConfigurationApi.AdaptersConfigurationIdGet(id);
            return Mapper.Map<AdapterConfiguration>(dp);
        }

        public async Task UpdateAdapter(AdapterConfiguration adapterConfiguration)
        {
            await MainApi.AdaptersConfigurationApi.AdaptersConfigurationPutAsync(Mapper.Map<AdapterConfigurationItem>(adapterConfiguration));
        }

        public async Task DeleteAdapter(int id)
        {
            throw new NotImplementedException("Внешние клиенты не могут удалять адаптеры");
        }

        #endregion

        #region DataPoints

        public IEnumerable<DataPointInfo> DataPoints =>
            MainApi.AdaptersConfigurationApi.AdaptersConfigurationListGet().Data
            .SelectMany(a => a.DataPoints
                .Select(did => MainApi.DataPointsApi.DataPointsIdGet(did.Id))
                .Select(dp => dp.DataPoint));

        public async Task<DataPointInfo?> GetDataPointValue(int id)
        {
            var dp = await MainApi.DataPointsApi.DataPointsIdGetAsync(id);
            return dp.DataPoint;
        }

        public async Task<List<DataPointInfo>> GetDataPointValues(List<int>? ids)
        {
            if (ids == null)
                return new List<DataPointInfo>();
            var dps = await MainApi.DataPointsApi.DataPointsIdsGetAsync(String.Join(",", ids.Select(id => id.ToString())));
            return dps.DataPoints;
        }

        public async Task UpdateDataPoint(int id, DataValue value)
        {
            await MainApi.DataPointsApi.DataPointsUpdatePutAsync(value, id);
        }

        public async Task UpdateDataPoints(IEnumerable<(int id, DataValue value)> updates)
        {
            await MainApi.DataPointsApi.DataPointsUpdateListPutAsync(
                new API.Main.Contracts.DataPoint.DataPointUpdateListQuery(updates.ToList()));
        }

        public void CreateTemporaryDataPoint(DataPointConfiguration config)
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
