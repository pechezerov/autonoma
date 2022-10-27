namespace Autonoma.Communication.Modbus
{
    public class ModbusRequestContext
    {
        public ModbusRequestContext(ModbusMap map, ModbusMapBucket bucket)
        {
            Map = map;
            Bucket = bucket;
        }

        public ModbusMap Map { get; private set; }
        public ModbusMapBucket Bucket { get; private set; }
        public ModbusQuery Query => Bucket.Query;
    }
}