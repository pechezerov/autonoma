using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using Newtonsoft.Json;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autonoma.UI.Configuration.ViewModels
{
    internal class AdapterViewModel : ViewModelBase, IAdapter
    {
        public AdapterViewModel(AdapterConfiguration configuration)
        {
            AdapterType = configuration.AdapterType;
            Name = configuration.Name;

            var adapterSettingsType = Type.GetType(configuration.AdapterType.AssemblyQualifiedSettingsTypeName);
            var datapointSettingsType = Type.GetType(configuration.AdapterType.AssemblyQualifiedDataPointSettingsTypeName);

            if (adapterSettingsType != null)
            {
                try
                {
                    Settings = JsonConvert.DeserializeObject(configuration.Settings, adapterSettingsType);
                }
                catch
                {
                }
            }

            foreach (var dp in configuration.DataPoints)
            {
                var datapoint = new DataPointViewModel(dp);
                if (datapointSettingsType != null)
                {
                    try
                    {
                        datapoint.Settings = JsonConvert.DeserializeObject(dp.Settings, datapointSettingsType);
                    }
                    catch
                    {
                    }
                }
                DataPoints.Add(datapoint);
            }
        }

        [Browsable(false)]
        [Reactive]
        public AdapterType? AdapterType { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Browsable(false)]
        [Reactive]
        public IList<IDataPoint> DataPoints { get; set; } = new ObservableCollection<IDataPoint>();

        [Reactive]
        public object? Settings { get; set; }
    }
}