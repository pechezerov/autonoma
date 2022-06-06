namespace Autonoma.Communication.Modbus
{
    public enum ModbusAdapterType
    {
        ModbusTcp,
        ModbusUdp
    }

    public class ModbusClientConfiguration
    {
        public ModbusAdapterType AdapterType { get; set; }
    }
}