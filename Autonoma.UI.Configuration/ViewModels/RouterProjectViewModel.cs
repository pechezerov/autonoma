using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class RouterProjectViewModel : ViewModelBase, IRouterProject
    {
        [Reactive]
        public IList<IRouter> Routers { get; set; } = new ObservableCollection<IRouter>();

        [Reactive]
        public object? SelectedTechnologyElement { get; set; }
    }
}