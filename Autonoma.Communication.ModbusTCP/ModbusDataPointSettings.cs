namespace Autonoma.Communication.Modbus
{
    public class ModbusDataPointSettings
    {
        public ModbusType RegisterType { get; set; }
        public ushort Address { get; set; }
        public int BitAddress { get; set; }
    }
}