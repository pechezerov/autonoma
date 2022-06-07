using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autonoma.UI.Configuration.ViewModels
{
    internal class RouterViewModel : ViewModelBase, IRouter
    {
        [ReadOnly(true)]
        public string? Name { get; set; }

        [ReadOnly(true)]
        public int? AdapterTypeId { get; set; }

        [Browsable(false)]
        public IList<IAdapter> Adapters { get; set; } = new ObservableCollection<IAdapter>();
    }
}