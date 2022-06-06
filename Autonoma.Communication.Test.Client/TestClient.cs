using Autonoma.Communication.Hosting;
using Autonoma.Communication.Modbus;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Autonoma.Communication.Test.Client
{
    public class TestClient : DataAdapterBase<TestClientConfiguration>
    {
        private WorkState _state;
        public override int AdapterTypeId => 2;
        public Random Randomizer { get; }
        public override WorkState State => _state;

        public TestClient(IDataPointService dps, AdapterConfiguration config) : base(dps, config)
        {
            Randomizer = new Random(DateTime.Now.Millisecond);
        }

        protected override async Task ExecuteAsync(System.Threading.CancellationToken cancellationToken)
        {
            _state = WorkState.On;

            while (!cancellationToken.IsCancellationRequested)
            {
                CancellationTokenSource cts = new CancellationTokenSource(1000);
                try
                {
                    await EmitTestData();
                    await Task.Delay(1000);
                }
                catch (Exception exception)
                {
                }
            }

            _state = WorkState.Off;
        }

        private async Task EmitTestData()
        {
            var updates = new List<(int, DataValue)>();
            var dpIds = Configuration.DataPoints.Select(p => p.Id).ToList();
            var dataPointValues = await DataPointService.GetDataPointValues(dpIds);
            foreach (var dataPointValue in dataPointValues)
            {
                var v = Randomizer.Next(5) - 2;
                var tv = new DataValue { Timestamp = DateTime.Now, Quality = DataQuality.Ok, Value = dataPointValue.Value.ValueDouble() + v };
                updates.Add((dataPointValue.DataPointId, tv));
            }
            await DataPointService.UpdateDataPoints(updates);
        }
    }
}
