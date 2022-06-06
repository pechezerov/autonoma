using Autonoma.App.Models;
using Autonoma.App.Services.RequestProvider;
using Autonoma.App.Services.Settings;
using Autonoma.Core.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.Adapter
{
    public class AdapterInfoService : IAdapterInfoService
    {
        private const string ApiUrlBase = "api/v1/adapter";

        private readonly IRequestProvider _requestProvider;
        private readonly ISettingsService _settings;

        public AdapterInfoService(IRequestProvider requestProvider, ISettingsService settings)
        {
            _requestProvider = requestProvider;
            _settings = settings;
        }

        public async Task<AdapterInfo> GetAdapterByIdAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}");
            return await _requestProvider.GetAsync<AdapterInfo>(uri, _settings.AuthAccessToken);
        }

        public async Task<List<AdapterInfo>> GetAllAdaptersAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, ApiUrlBase);
            var adapterList = await _requestProvider.GetAsync<List<AdapterInfo>>(uri, _settings.AuthAccessToken);
            return adapterList;
        }

        public async Task<List<AdapterStateInfo>> GetAdapterStatesAsync()
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/state");
            var adapterStateList = await _requestProvider.GetAsync<List<AdapterStateInfo>>(uri, _settings.AuthAccessToken);
            return adapterStateList;
        }

        public async Task StartAdapterByIdAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}/start");
            await _requestProvider.PostAsync(uri, _settings.AuthAccessToken);
        }

        public async Task StopAdapterByIdAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}/stop");
            await _requestProvider.PostAsync(uri, _settings.AuthAccessToken);
        }

        public async Task UpdateAdapterAsync(AdapterInfo model)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{model.Id}");
            await _requestProvider.PutAsync<AdapterInfo>(uri, model, _settings.AuthAccessToken);
        }

        public async Task RemoveAdapterAsync(int id)
        {
            var uri = UriHelper.CombineUri(GlobalSetting.Instance.BaseEndpoint, $"{ApiUrlBase}/{id}");
            await _requestProvider.DeleteAsync(uri, _settings.AuthAccessToken);
        }
    }
}
