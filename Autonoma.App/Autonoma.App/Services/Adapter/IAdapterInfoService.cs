using Autonoma.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.Adapter
{
    public interface IAdapterInfoService
    {
        Task<List<AdapterInfo>> GetAllAdaptersAsync();
        Task<AdapterInfo> GetAdapterByIdAsync(int id);
        Task<List<AdapterStateInfo>> GetAdapterStatesAsync();

        Task UpdateAdapterAsync(AdapterInfo model);

        Task RemoveAdapterAsync(int id);

        Task StopAdapterByIdAsync(int id);
        Task StartAdapterByIdAsync(int id);
    }
}
