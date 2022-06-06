using Autonoma.Domain;
using Autonoma.Domain.Abstractions;
using Autonoma.Domain.Entities;
using Autonoma.Extensibility.Shared.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.Communication.Hosting.Local
{
    public class AdapterHostService : IAdapterService
    {
        private bool disposedValue;

        public List<IDataAdapter> Adapters { get; private set; } = new List<IDataAdapter>();

        IEnumerable<IDataAdapter> IAdapterService.Adapters => Adapters;

        public IDataPointService DataPointService { get; }

        public IExtensibilityService ExtService { get; }

        public AdapterHostService(IDataPointService dataPointService, IExtensibilityService extService)
        {
            DataPointService = dataPointService;
            ExtService = extService;
        }

        public async Task Initialize(List<AdapterConfiguration> adapterConfigurations)
        {
            foreach (var adapterConfiguration in adapterConfigurations)
            {
                await UpdateAdapter(adapterConfiguration);
            }
        }

        public async Task UpdateAdapter(AdapterConfiguration adapterConfiguration)
        {
            // actual state
            WorkState? initialState = null;

            var existingAdapter = Adapters.FirstOrDefault(a => a.Id == adapterConfiguration.Id);
            if (existingAdapter != null)
                await DeleteAdapter(adapterConfiguration.Id);

            // create adapter
            var newAdapter = CreateAdapter(adapterConfiguration);
            Adapters.Add(newAdapter);

            // start adapter again if it worked before
            if (initialState != null && initialState != WorkState.Off)
                await newAdapter.StartAsync(System.Threading.CancellationToken.None);
        }

        public async Task DeleteAdapter(int id)
        {
            var existingAdapter = Adapters.FirstOrDefault(a => a.Id == id);
            if (existingAdapter != null)
            {
                // stop adapter
                await existingAdapter.StopAsync(System.Threading.CancellationToken.None);

                // remove adapter
                existingAdapter.Dispose();
                Adapters.Remove(existingAdapter);
            }
        }

        private IDataAdapter CreateAdapter(AdapterConfiguration adapterConfiguration)
        {
            IAdapterPlugin? targetPlugin = null;
            string? pluginName = adapterConfiguration.AdapterType?.AssemblyQualifiedAdapterTypeName;
            if (ExtService != null && pluginName != null)
            {
                targetPlugin = ExtService.GetInstance<IAdapterPlugin>(pluginName);
                return new PluginAdapter(DataPointService, adapterConfiguration, targetPlugin);
            }

            return new IdleAdapter(DataPointService, null);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

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
    }
}
