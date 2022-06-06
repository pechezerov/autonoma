using PrettyScreen.Communication;
using PrettyScreen.Communication.Modbus;
using PrettyScreen.Configuration;
using PrettyScreen.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PrettyScreen.Core
{
    public class SimpleCommunicationService : ICommunicationService
    {
        private bool disposedValue;

        public SimpleCommunicationService(IDataPointService dataPointService, IDataStore<AdapterConfiguration> commModel)
        {
            Assembly.Load(Assembly.GetAssembly(typeof(ModbusClient)).GetName());

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToArray();
            var implTypes = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(IDataAdapter).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .ToArray();

            foreach (var implType in implTypes)
            {
                foreach (var adapterTypeAttribute in implType.GetCustomAttribute<AdapterTypeAttribute>().AdapterTypes)
                {
                    AdapterTypes.Add(adapterTypeAttribute, implType);
                }
            }

            foreach (var adapterModel in commModel.Items)
            {
                var adapter = CreateAdapter(dataPointService, adapterModel);
                Adapters.Add(adapter);
                adapter.Start();
            }
        }

        private IDataAdapter CreateAdapter(IDataPointService dataPointService, AdapterConfiguration adapterModel)
        {
            Type adapterType = null;
            if (AdapterTypes.TryGetValue(adapterModel.Type, out adapterType))
            {
                var adapterInstance = Activator.CreateInstance(adapterType, new object[] { dataPointService, adapterModel, adapterModel.Type });
                return adapterInstance as IDataAdapter;
            }
            return new IdleAdapter(dataPointService, adapterModel, AdapterType.Idle);
        }

        public List<IDataAdapter> Adapters { get; private set; } = new List<IDataAdapter>();

        IEnumerable<IDataAdapter> ICommunicationService.Adapters => Adapters;

        public Dictionary<AdapterType, Type> AdapterTypes { get; } = new Dictionary<AdapterType, Type>();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var adapter in Adapters)
                    {
                        adapter.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
