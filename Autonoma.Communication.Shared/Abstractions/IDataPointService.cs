﻿using Autonoma.Domain;
using Autonoma.Domain.Entities;

namespace Autonoma.Communication.Hosting
{
    public interface IDataPointService
    {
        /// <summary>
        /// Возвращает текущее состояние указанной точки данных
        /// </summary>
        Task<DataPointInfo?> GetDataPointValue(int id);

        /// <summary>
        /// Возвращает текущее состояние указанных точек данных
        /// </summary>
        Task<List<DataPointInfo>> GetDataPointValues(IEnumerable<int>? ids);

        /// <summary>
        /// Обновляет состояние указанной точки данных
        /// </summary>
        Task UpdateDataPoint(int id, DataValue value);

        /// <summary>
        /// Массово обновляет состояние указанных точек данных
        /// </summary>
        Task UpdateDataPoints(IEnumerable<(int id, DataValue value)> updates);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        void CreateSystemDataPoint(DataPointConfiguration config);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        void RemoveDataPoint(int id);
    }
}