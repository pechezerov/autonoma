using Autonoma.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.DataPoint
{
    public interface IDataPointInfoService
    {
        Task<List<DataPointInfo>> GetAllDataPointsAsync();
        Task<DataPointInfo> GetDataPointByIdAsync(int id);
        Task<DataValueInfo> GetCurrentValueAsync(int id);

        Task CreateOrUpdateDataPointAsync(DataPointInfo model);

        Task RemoveDataPointAsync(int id);
    }
}
