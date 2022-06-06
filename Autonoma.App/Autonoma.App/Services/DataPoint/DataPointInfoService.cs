using Autonoma.App.Models;
using Autonoma.App.Services.RequestProvider;
using Autonoma.App.Services.Settings;
using Autonoma.Core.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.DataPoint
{
    public class DataPointInfoService : IDataPointInfoService
    {
        private const string ApiUrlBase = "api/v1/datapoint";

        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settings;

        public DataPointInfoService(IRequestProvider requestProvider, ISettingsService settings)
        {
            _requestProvider = requestProvider;
            _settings = settings;
        }

        public async Task<DataPointInfo> GetDataPointByIdAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}");
            return await _requestProvider.GetAsync<DataPointInfo>(uri, _settings.AuthAccessToken);
        }

        public async Task<List<DataPointInfo>> GetAllDataPointsAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, ApiUrlBase);
            var adapterList = await _requestProvider.GetAsync<List<DataPointInfo>>(uri, _settings.AuthAccessToken);
            return adapterList;
        }

        public async Task<DataValueInfo> GetCurrentValueAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}/value");
            return await _requestProvider.GetAsync<DataValueInfo>(uri, _settings.AuthAccessToken);
        }

        public async Task CreateOrUpdateDataPointAsync(DataPointInfo model)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, ApiUrlBase);
            await _requestProvider.PutAsync<DataPointInfo>(uri, model, _settings.AuthAccessToken);
        }

        public async Task RemoveDataPointAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}");
            await _requestProvider.DeleteAsync(uri, _settings.AuthAccessToken);
        }
    }
}
