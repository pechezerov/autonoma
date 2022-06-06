using Autonoma.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.Adapter
{
    public class AdapterMockService : IAdapterInfoService
    {
        private AdapterInfo MockAdapterInfo = new AdapterInfo
        {
            
        };

        private AdapterStateInfo MockAdapterStateInfo = new AdapterStateInfo
        {

        };

        public async Task<AdapterInfo> GetAdapterByIdAsync(int adapterId)
        {
            await Task.Delay(10); 
            return MockAdapterInfo;
        }

        public async Task<List<AdapterInfo>> GetAllAdaptersAsync()
        {
            await Task.Delay(10); 
            return new List<AdapterInfo> { MockAdapterInfo };
        }

        public async Task<List<AdapterStateInfo>> GetAdapterStatesAsync()
        {
            await Task.Delay(10);
            return new List<AdapterStateInfo> { MockAdapterStateInfo };
        }

        public async Task StartAdapterByIdAsync(int id)
        {
            await Task.Delay(10);
        }

        public async Task StopAdapterByIdAsync(int id)
        {
            await Task.Delay(10);
        }

        public async Task UpdateAdapterAsync(AdapterInfo model)
        {
            await Task.Delay(10);
        }

        public async Task RemoveAdapterAsync(int id)
        {
            await Task.Delay(10);
        }
    }
}
