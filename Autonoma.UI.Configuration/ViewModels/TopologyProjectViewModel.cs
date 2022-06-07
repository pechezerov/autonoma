using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class TopologyProjectViewModel : ViewModelBase, ITopologyProject
    {
        public IEnumerable<IModelElement> Elements { get; set; }

        public string Name { get; set; }

        public TopologyProjectViewModel()
        {
            Elements = new ObservableCollection<IModelElement>();
        }
    }
}