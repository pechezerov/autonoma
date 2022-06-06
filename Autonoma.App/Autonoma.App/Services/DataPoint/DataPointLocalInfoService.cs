using Autonoma.App.Models;
using Autonoma.Communication.Hosting;
using Autonoma.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.App.Services.DataPoint
{
    public class DataPointLocalInfoService : IDataPointInfoService
    {
        private readonly IDataPointHostService _dataPointHostService;
        
        public DataPointLocalInfoService(IDataPointHostService dataPointHostService)
        {
            _dataPointHostService = dataPointHostService;
        }

        public Task<DataPointInfo> GetDataPointByIdAsync(int id)
        {
            return Task.Run(() =>
            {
                var dataPoint = _dataPointHostService.DataPoints.FirstOrDefault(a => a.Id == id);
                return new DataPointInfo { };
            });
        }

        public Task<List<DataPointInfo>> GetAllDataPointsAsync()
        {
            return Task.Run(() =>
            {
                var dataPointList = _dataPointHostService.DataPoints;
                return dataPointList.Select(a => new DataPointInfo { })
                    .ToList();
            });
        }

        public Task<DataValueInfo> GetCurrentValueAsync(int id)
        {
            return Task.Run(() =>
            {
                var dataPoint = _dataPointHostService.DataPoints.FirstOrDefault(a => a.Id == id);
                if (dataPoint != null)
                {
                    var dateValue = dataPoint.Current;
                    return new DataValueInfo { };
                }
                else
                {
                    return null;
                }
            });
        }

        public Task CreateOrUpdateDataPointAsync(DataPointInfo model)
        {
            return Task.Run(() =>
            {
                ;
            });
        }

        public Task RemoveDataPointAsync(int id)
        {
            return Task.Run(() =>
            {
                _dataPointHostService.RemoveDataPoint(id);
            });
        }
    }
}
