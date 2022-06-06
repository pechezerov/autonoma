namespace Autonoma.Communication.Modbus
{
    public class ModbusQuery
    {
        public ushort StartRegister { get; internal set; }
        public ushort RequestLength { get; internal set; }
        public ModbusType Type { get; set; }
    }
}