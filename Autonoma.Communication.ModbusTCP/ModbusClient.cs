using Autonoma.Communication.Hosting;
using Autonoma.Core.Extensions;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using NModbus;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Autonoma.Communication.Modbus
{
    public class ModbusClient : DataAdapterBase<ModbusClientConfiguration>
    {
        public override int AdapterTypeId => 3;

        private ModbusMap Map { get; }

        private bool UseUDP { get; }

        public byte SlaveAddress { get; }

        public ModbusClient(IDataPointService dps, AdapterConfiguration config) : base(dps, config)
        {
            Map = new ModbusMap(config);
            UseUDP = Options.AdapterType == ModbusAdapterType.ModbusUdp;
            SlaveAddress = Options.Address;
        }

        protected override Task ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                IDisposable client = null;
                IModbusMaster master = null;

                try
                {
                    if (!UseUDP)
                    {
                        client = new TcpClient();
                        ((TcpClient)client).Connect(IPAddress.Parse(Options.IpAddress), Options.Port);
                        var factory = new ModbusFactory();
                        master = factory.CreateMaster(((TcpClient)client));
                    }
                    else
                    {
                        client = new UdpClient();
                        ((UdpClient)client).Connect(IPAddress.Parse(Options.IpAddress), Options.Port);
                        var factory = new ModbusFactory();
                        master = factory.CreateMaster(((UdpClient)client));
                    }

                    while (true)
                    {
                        var requestContext = Map.GetQueryContext();
                        if (requestContext == null)
                            break;
                        var query = requestContext.Query;
                        ushort[] result = new ushort[0];

                        if (query.Type == ModbusType.Holding)
                            result = master.ReadHoldingRegisters(SlaveAddress, query.StartRegister, query.RequestLength);
                        else if (query.Type == ModbusType.Input)
                            result = master.ReadInputRegisters(SlaveAddress, query.StartRegister, query.RequestLength);

                        OnReceiveData(requestContext, result);
                    }

                    Error = null;
                }
                catch (Exception ex)
                {
                    ErrorCounter++;
                    Error = ex;
                }
                finally
                {
                    client?.Dispose();
                    master?.Dispose();
                }
            });
        }

        private void OnReceiveData(ModbusRequestContext requestContext, ushort[] result)
        {
            bool sameSize = requestContext.Bucket.Length == result.Length;
            if (sameSize)
            {
                var dt = DateTime.Now;
                requestContext.Bucket.LastUpdateTime = dt;

                foreach (var point in requestContext.Bucket.Points)
                {
                    var valueLength = (ushort)Math.Ceiling((double)point.Length / 2.0);
                    var pointRawData = result.SubArray<ushort>(point.Address - requestContext.Bucket.StartAddress.Value, valueLength);
                    var dataValue = DataValue.NoData;

                    switch (point.ValueType)
                    {
                        case TypeCode.Boolean:
                            var bitSetVal = pointRawData[0];
                            var bitVal = bitSetVal.GetBit(point.BitAddress);
                            break;
                        case TypeCode.Double:
                            var dVal = NModbus.Utility.ModbusUtility.GetDouble(pointRawData[0], pointRawData[1], pointRawData[2], pointRawData[3]);
                            dataValue = new DataValue(dVal, dt);
                            break;
                        case TypeCode.Int16:
                            dataValue = new DataValue((short)pointRawData[0], dt);
                            break;
                        case TypeCode.Int32:
                            var intVal = (int)NModbus.Utility.ModbusUtility.GetUInt32(pointRawData[0], pointRawData[1]);
                            dataValue = new DataValue(intVal, dt);
                            break;
                        case TypeCode.Single:
                            var sVal = (int)NModbus.Utility.ModbusUtility.GetSingle(pointRawData[0], pointRawData[1]);
                            dataValue = new DataValue(sVal, dt);
                            break;
                        case TypeCode.UInt16:
                            dataValue = new DataValue(pointRawData[0], dt);
                            break;
                        case TypeCode.UInt32:
                            var uintVal = NModbus.Utility.ModbusUtility.GetUInt32(pointRawData[0], pointRawData[1]);
                            dataValue = new DataValue(uintVal, dt);
                            break;
                        default:
                            break;
                    }

                    DataPointService.UpdateDataPoint(point.Id, dataValue);
                }
            }
        }
    }
}
