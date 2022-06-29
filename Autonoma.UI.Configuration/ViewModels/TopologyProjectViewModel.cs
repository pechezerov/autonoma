using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class TopologyProjectViewModel : ViewModelBase, ITopologyProject
    {
        public ObservableCollection<IModelElement> Elements { get; set; }
        IList<IModelElement> IModelElement.Elements => Elements;

        [Reactive]
        public IModelElement? SelectedElement { get; set; }

        public string Name { get; set; } = "";

        public ReactiveCommand<Unit, Unit> CreateElementCommand { get; }

        public ReactiveCommand<Unit, Unit> RemoveElementCommand { get; }

        public IModelElement? Parent => null;

        public bool AllowEditElements => true;

        public IList<IModelAttribute> Attributes => Array.Empty<IModelAttribute>();

        public IObservable<bool>? CanCreateElement;
        public IObservable<bool>? CanRemoveElement;

        public TopologyProjectViewModel()
        {
            Elements = new ObservableCollection<IModelElement>();
            CreateElementCommand = ReactiveCommand.CreateFromTask(CreateElement, CanCreateElement);
            RemoveElementCommand = ReactiveCommand.CreateFromTask(RemoveElement, CanRemoveElement);
        }

        private Task RemoveElement()
        {
            return Task.Run(() =>
            {
                if (SelectedElement != null)
                {
                    if (SelectedElement.Parent != null)
                        SelectedElement.Parent.Elements.Remove(SelectedElement);
                    else
                        Elements.Remove(SelectedElement);

                    SelectedElement = null;
                }
            });
        }

        private Task CreateElement()
        {
            return Task.Run(() =>
            {
                var newElement = new SimpleModelElementViewModel(ModelElementConfiguration.CreateNew());
                Elements.Add(newElement);
            });
        }

        public void AddElement(IModelElement element)
        {
            Elements.Add(element);
        }
    }
}