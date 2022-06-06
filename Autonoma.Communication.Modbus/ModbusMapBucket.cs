using System;
using System.Collections.Generic;
using System.Linq;

namespace Autonoma.Communication.Modbus
{
    public class ModbusMapBucket
    {
        public ModbusMapBucket(ModbusType registerType, TimeSpan refreshInterval)
        {
            RefreshInterval = refreshInterval;
            Points = new List<ModbusDataPointConfiguration>();
            StartAddress = null;
        }

        public List<ModbusDataPointConfiguration> Points { get; }
        public bool IsFull => Length >= MaxLength;
        public bool IsEmpty => !Points.Any();

        public ushort Length => (ushort?)(EndAddress - StartAddress) ?? (ushort)0;
        public ushort? StartAddress { get; set; }
        public ushort? EndAddress { get; set; }

        public DateTime LastUpdateTime { get; internal set; }
        public DateTime PlannedUpdateTime => LastUpdateTime + RefreshInterval;
        public TimeSpan RefreshInterval { get; private set; }

        public ModbusQuery Query => new ModbusQuery
        {
            StartRegister = this.StartAddress.Value,
            RequestLength = this.Length,
            Type = this.RegisterType
        };

        public int MaxLength => 200;
        public ModbusType RegisterType { get; private set; }

        internal void Add(ModbusDataPointConfiguration modbusConfig)
        {
            if (!Points.Any())
                StartAddress = modbusConfig.Address;

            Points.Add(modbusConfig);

            EndAddress = Points.Max(p => (ushort)(p.Address + p.Length));
        }

        internal bool CanAdd(ModbusDataPointConfiguration modbusConfig)
        {
            if (!Points.Any())
                return true;
            return (modbusConfig.Address + modbusConfig.Length - StartAddress) < MaxLength;
        }
    }
}