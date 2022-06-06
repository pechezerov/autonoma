using NModbus;
using Autonoma.Communication.Hosting;
using Autonoma.Core.Extensions;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace Autonoma.Communication.Modbus
{
    public class ModbusClient : DataAdapterBase
    {
        private Timer InterrogationTimer { get; }
        private ModbusMap Map { get; }

        public AdapterType AdapterType { get; }

        private bool UseUDP { get; }

        public byte SlaveAddress { get; }

        public ModbusClient(IDataPointHost dps, AdapterConfiguration config, AdapterType adapterType)
          : base(dps, config, adapterType)
        {
            Map = new ModbusMap(config);
            AdapterType = adapterType;
            UseUDP = adapterType == AdapterType.ModbusUdp;

            if (Byte.TryParse(Configuration.Address, out byte slaveAddr))
                SlaveAddress = slaveAddr;

            InterrogationTimer = new Timer();
            InterrogationTimer.Elapsed += DoRequest;
            InterrogationTimer.Start();
        }

        public override void Start()
        {
            InterrogationTimer.Enabled = true;
        }

        private void DoRequest(object sender, ElapsedEventArgs e)
        {
            Busy = true;
            InterrogationTimer.Enabled = false;

            IDisposable client = null;
            IModbusMaster master = null;

            try
            {

                if (AdapterType == AdapterType.ModbusTcp)
                {
                    client = new TcpClient();
                    ((TcpClient)client).Connect(IPAddress.Parse(Configuration.IpAddress), Configuration.Port);
                    var factory = new ModbusFactory();
                    master = factory.CreateMaster(((TcpClient)client));
                }
                else if (AdapterType == AdapterType.ModbusUdp)
                {
                    client = new UdpClient();
                    ((UdpClient)client).Connect(IPAddress.Parse(Configuration.IpAddress), Configuration.Port);
                    var factory = new ModbusFactory();
                    master = factory.CreateMaster(((UdpClient)client));
                }
                else
                {
                    throw new NotImplementedException();
                }

                while (true)
                {
                    var requestContext = Map.GetQueryContext();
                    if (requestContext == null)
                        break;
                    var query = requestContext.Query;
                    ushort[] result = new ushort[0];
                    if (query.Type == ModbusType.Holding)
                    {
                        result = master.ReadHoldingRegisters(SlaveAddress, query.StartRegister, query.RequestLength);
                    }
                    else if (query.Type == ModbusType.Input)
                    {
                        result = master.ReadInputRegisters(SlaveAddress, query.StartRegister, query.RequestLength);
                    }

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
                Busy = false;
            }
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
                    var dataPoint = DataPointService.GetDataPoint(point.Id);
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

                    dataPoint.Update(dataValue);
                }
            }
        }

        public override void Stop()
        {
            InterrogationTimer.Enabled = false;
        }

        public override WorkState State => (Busy || InterrogationTimer.Enabled) ? WorkState.On : WorkState.Off;


        private bool busy;

        public bool Busy
        {
            get => busy;
            private set
            {
                busy = value;
                if (InterrogationTimer != null)
                    InterrogationTimer.Enabled = !busy;
            }
        }
    }
}
