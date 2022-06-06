using Autonoma.App.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.DataPoint
{
    public class DataPointMockService : IDataPointInfoService
    {
        private DataPointInfo MockDataPointInfo = new DataPointInfo
        {
            
        };

        private DataValueInfo MockDataValueInfo = new DataValueInfo
        {

        };

        public async Task<DataPointInfo> GetDataPointByIdAsync(int adapterId)
        {
            await Task.Delay(10); 
            return MockDataPointInfo;
        }

        public async Task<List<DataPointInfo>> GetAllDataPointsAsync()
        {
            await Task.Delay(10); 
            return new List<DataPointInfo> { MockDataPointInfo };
        }

        public async Task CreateOrUpdateDataPointAsync(DataPointInfo model)
        {
            await Task.Delay(10);
        }

        public async Task<DataValueInfo> GetCurrentValueAsync(int id)
        {
            await Task.Delay(10);
            return MockDataValueInfo;
        }

        public async Task RemoveDataPointAsync(int id)
        {
            await Task.Delay(10);
        }
    }
}
