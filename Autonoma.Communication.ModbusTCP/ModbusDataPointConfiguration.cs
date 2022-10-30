using Autonoma.Core;
using Autonoma.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Runtime.Intrinsics.Arm;

namespace Autonoma.Communication.Modbus
{
    internal record ModbusDataPointConfiguration
    {
        public ModbusDataPointConfiguration()
        {
        }

        public ModbusDataPointConfiguration(DataPointConfiguration c)
        {
            Id = c.Id;
            ValueType = c.Type;

            try
            {
                var settings = JsonConvert.DeserializeObject<ModbusDataPointSettings>(c.Settings);
                RegisterType = settings.RegisterType;
                Address = settings.Address;
                BitAddress = settings.BitAddress;
            }
            catch
            {
            }
        }

        public int Id { get; }
        public TypeCode ValueType { get; }
        public ModbusType RegisterType { get; set; }
        public ushort Address { get; set; }
        public int BitAddress { get; set; }
        public ushort Length => Globals.GetTypeSize(ValueType);

        public bool Invalid { get; set; }
    }
}