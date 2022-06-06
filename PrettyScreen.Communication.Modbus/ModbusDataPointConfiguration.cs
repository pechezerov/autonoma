using PrettyScreen.Configuration;
using PrettyScreen.Core;
using System;

namespace PrettyScreen.Communication.Modbus
{
    public record ModbusDataPointConfiguration
    {
        public ModbusDataPointConfiguration(Guid pointId, TypeCode type)
        {
            Id = pointId;
            ValueType = type;
        }

        public static ModbusDataPointConfiguration Create(DataPointConfiguration baseConfig)
        {
            var result = new ModbusDataPointConfiguration(baseConfig.Id, baseConfig.Type);

            try
            {
                string param = baseConfig.Mapping;
                string[] paramParts = param.Split(new string[] { "\\", "/", ":", ";", " " }, StringSplitOptions.None);
                if (paramParts.Length < 2)
                    throw new InvalidOperationException("Неверный формат записи: недостаточно элементов");
                var address = paramParts[0];
                if (address.ToLower().Equals("i"))
                    result.RegisterType = ModbusType.Input;
                else if (address.ToLower().Equals("h"))
                    result.RegisterType = ModbusType.Holding;
                result.Address = UInt16.Parse(paramParts[1]);
                if (paramParts.Length > 2)
                    result.BitAddress = Int32.Parse(paramParts[2]);
            }
            catch
            {
                result.Invalid = true;
            }

            return result;
        }

        public Guid Id { get; }
        public TypeCode ValueType { get; }
        public ModbusType RegisterType { get; set; }
        public ushort Address { get; set; }
        public int BitAddress { get; set; }
        public ushort Length => Globals.GetTypeSize(ValueType);

        public bool Invalid { get; set; }
    }
}