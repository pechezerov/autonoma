using PrettyScreen.Configuration;
using PrettyScreen.Core.Attributes;
using PrettyScreen.Core.Communication;
using System;
using System.Timers;

namespace PrettyScreen.Core
{
    [AdapterType(AdapterType.Test)]
    public class TestAdapter : DataAdapterBase
    {
        private Timer workTimer = new Timer(1000);

        public TestAdapter(IDataPointService dps, AdapterConfiguration config, AdapterType adapterType)
            : base(dps, config, adapterType)
        {
            Randomizer = new Random(DateTime.Now.Millisecond);
            workTimer.Elapsed += EmitTestData;
            workTimer.Start();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            foreach (var point in DataPointService.DataPoints)
            {
                var v = Randomizer.NextDouble();
                var tv = new DataValue { Timestamp = DateTime.Now, Quality = DataQuality.Ok, Value = v };
                DataPointService.UpdateDataPoint(point.Id, tv);
            }
        }

        private void EmitTestData(object sender, ElapsedEventArgs e)
        {
            workTimer.Enabled = false;

            try
            {
                foreach (var point in Configuration.DataPoints)
                {
                    var dataPoint = DataPointService.GetDataPoint(point.Id);
                    var v = Randomizer.NextDouble() * 0.02 - 0.01;
                    var tv = new DataValue { Timestamp = DateTime.Now, Quality = DataQuality.Ok, Value = dataPoint.Current.ValueDouble + v};
                    DataPointService.UpdateDataPoint(point.Id, tv);
                }
            }
            finally
            {
                workTimer.Enabled = true;
            }
        }

        public override WorkState State => workTimer.Enabled ? WorkState.On : WorkState.Off;

        public Random Randomizer { get; }

        public override void Start()
        {
            workTimer.Enabled = true;
        }

        public override void Stop()
        {
            workTimer.Enabled = false;
        }

        public override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    workTimer.Stop();
                    workTimer.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}