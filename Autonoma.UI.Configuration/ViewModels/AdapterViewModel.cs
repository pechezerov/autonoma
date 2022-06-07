using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using ReactiveUI.Fody.Helpers;
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
            Address = configuration.Address;
            IpAddress = configuration.IpAddress;
            Name = configuration.Name;
            Configuration = configuration.Configuration;
            Port = configuration.Port;

            foreach (var dp in configuration.DataPoints)
                DataPoints.Add(new DataPointViewModel(dp));
        }

        [Browsable(false)]
        [Reactive]
        public AdapterType? AdapterType { get; set; }

        [Reactive]
        public string Address { get; set; }

        [Reactive]
        public string IpAddress { get; set; }

        [Reactive]
        public string Name { get; set; }

        [Reactive]
        public string Configuration { get; set; }

        [Reactive]
        public int Port { get; set; }

        [Browsable(false)]
        [Reactive]
        public IList<IDataPoint> DataPoints { get; set; } = new ObservableCollection<IDataPoint>();
    }
}