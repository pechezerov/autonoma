using Autonoma.Communication.Hosting;
using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using Newtonsoft.Json;
using System;

namespace Autonoma.Communication
{
    public abstract class DataAdapterBase<TOptions> : HostedService, IDataAdapter where TOptions : class
    {
        protected bool disposedValue;

        public abstract int AdapterTypeId { get; }

        public IDataPointService DataPointService { get; }

        public Exception? Error { get; set; }

        public int ErrorCounter { get; set; }

        public int Id => Configuration.Id;

        public TOptions Options { get; set; }

        public virtual WorkState State { get; protected set; }

        protected AdapterConfiguration Configuration { get; set; }

        protected DataAdapterBase(IDataPointService dps, AdapterConfiguration config)
        {
            DataPointService = dps;
            Configuration = config;

            if (config.AdapterTypeId != AdapterTypeId)
                throw new InvalidOperationException();
            if (config.AdapterType.AssemblyQualifiedAdapterTypeName == null)
                throw new InvalidOperationException();
            if (GetType() != Type.GetType(config.AdapterType.AssemblyQualifiedAdapterTypeName))
                throw new InvalidOperationException();

            var options = JsonConvert.DeserializeObject(config.Configuration ?? "{}", typeof(TOptions)) as TOptions;
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            Options = options;
        }

        public void Dispose()
        {
        }
    }
}
