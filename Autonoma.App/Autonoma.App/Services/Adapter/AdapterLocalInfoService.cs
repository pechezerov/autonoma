using Autonoma.App.Models;
using Autonoma.Communication.Hosting;
using Autonoma.Core.Util;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.App.Services.Adapter
{
    public class AdapterLocalInfoService : IAdapterInfoService
    {
        private readonly IAdapterHostService _adapterHostService;

        public AdapterLocalInfoService(IAdapterHostService adapterHostService)
        {
            _adapterHostService = adapterHostService;
        }

        public Task<AdapterInfo> GetAdapterByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                var adapter = _adapterHostService.Adapters.FirstOrDefault(a => a.Id == id);
                return new AdapterInfo { };
            });
        }

        public Task<List<AdapterInfo>> GetAllAdaptersAsync()
        {
            return Task.Run(() =>
            {
                var adapterList = _adapterHostService.Adapters;
                return adapterList.Select(a => new AdapterInfo { })
                    .ToList();
            });
        }

        public async Task<List<AdapterStateInfo>> GetAdapterStatesAsync()
        {
            return (await GetAllAdaptersAsync())
                .Select(a => a.State)
                .ToList();
        }

        public Task StartAdapterByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                var adapter = _adapterHostService.Adapters.FirstOrDefault(a => a.Id == id);
                if (adapter != null)
                    adapter.Start();
            });
        }

        public Task StopAdapterByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                var adapter = _adapterHostService.Adapters.FirstOrDefault(a => a.Id == id);
                if (adapter != null)
                    adapter.Stop();
            });
        }

        public Task UpdateAdapterAsync(AdapterInfo model)
        {
            return Task.Run(() =>
            {
                var cfg = new AdapterConfiguration
                {

                };
                _adapterHostService.UpdateAdapter(cfg);
            });
        }

        public Task RemoveAdapterAsync(int id)
        {
            return Task.Run(() =>
            {
                StopAdapterByIdAsync(id)
                    .ContinueWith(r => { });
                var adapter = _adapterHostService.Adapters.FirstOrDefault(a => a.Id == id);
                return new AdapterInfo { };
            });
        }
    }
}
