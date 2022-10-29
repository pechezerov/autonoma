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

            var settingsType = Type.GetType(configuration.AdapterType.AssemblyQualifiedSettingsTypeName);
            if (settingsType != null)
                Settings = JsonConvert.DeserializeObject(configuration.Settings, settingsType);

            foreach (var dp in configuration.DataPoints)
                DataPoints.Add(new DataPointViewModel(dp));
        }

        [Browsable(false)]
        [Reactive]
        public AdapterType? AdapterType { get; set; }

        [Reactive]
        public string Address { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Browsable(false)]
        [Reactive]
        public IList<IDataPoint> DataPoints { get; set; } = new ObservableCollection<IDataPoint>();

        [Reactive]
        public object Settings { get; set; }
    }
}