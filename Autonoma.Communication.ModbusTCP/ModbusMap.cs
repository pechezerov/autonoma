using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autonoma.Communication.Modbus
{

    public class ModbusMap
    {
        public double RefreshInterval { get; } = 2000;
        
        public ModbusMap(AdapterConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));
            if (config.DataPoints == null)
                throw new ArgumentNullException(nameof(config.DataPoints));

            var modbusConfigs = config.DataPoints.Select(c => new ModbusDataPointConfiguration(c))
                .Where(c => !c.Invalid)
                .ToList();

            Buckets = new List<ModbusMapBucket>();

            foreach (var modbusConfigGroup in modbusConfigs.GroupBy(mc => mc.RegisterType))
            {
                var type = modbusConfigGroup.Key;
                var bucket = new ModbusMapBucket(type, TimeSpan.FromMilliseconds(RefreshInterval));
                foreach (var modbusConfig in modbusConfigGroup.OrderBy(mc => mc.Address))
                {
                    if (bucket.CanAdd(modbusConfig))
                        bucket.Add(modbusConfig);
                    if (bucket.IsFull)
                        Buckets.Add(bucket);
                }

                if (!bucket.IsEmpty)
                    Buckets.Add(bucket);
            }
        }

        public List<ModbusMapBucket> Buckets { get; }

        public ModbusRequestContext GetQueryContext(DateTime? contextTime = null)
        {
            var actualBuckets = Buckets.Where(b => b.PlannedUpdateTime <= (contextTime ?? DateTime.Now));
            var actualBucket = actualBuckets.FirstOrDefault();
            if (actualBucket != null)
                return new ModbusRequestContext(this, actualBucket);
            else 
                return null;
        }
    }
}