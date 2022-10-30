namespace Autonoma.Communication.Modbus
{
    public enum ModbusAdapterType
    {
        ModbusTcp,
        ModbusUdp
    }

    public class ModbusClientSettings
    {
        public ModbusAdapterType AdapterType { get; set; }

        public bool UseUDP { get; }

        public byte Address { get; } = 1;

        public string IpAddress { get; set; } = "";

        public int Port { get; set; } = 502;
    }
}