using PrettyScreen.Configuration;
using System;
using System.Collections.Generic;

namespace PrettyScreen.Core.Communication
{
    public abstract class DataAdapterBase : IDataAdapter
    {
        protected bool disposedValue;

        public DataAdapterBase(IDataPointService dps, AdapterConfiguration config, AdapterType adapterType)
        {
            DataPointService = dps;
            Configuration = config;
        }

        public IDataPointService DataPointService { get; }
        protected AdapterConfiguration Configuration { get; private set; }

        public virtual WorkState State { get; }

        public Guid Id => Configuration.Id;
        public Exception Error { get; set; }
        public int ErrorCounter { get; set; }

        public abstract void Start();
        public abstract void Stop();

        public virtual void Dispose(bool disposing)
        {
            /*
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }*/
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }
    }
}
