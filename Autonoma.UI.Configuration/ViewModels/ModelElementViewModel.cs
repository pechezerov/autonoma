using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Autonoma.UI.Configuration.ViewModels
{
    public abstract class ModelElementViewModel : ViewModelBase, IModelElement
    {
        public string Name 
        { 
            get => Configuration.Name;
            set 
            { 
                Configuration.Name = value; 
                this.RaisePropertyChanged(nameof(Name)); 
            }
        }

        [ReadOnly(true)]
        public string TemplateName
        {
            get => Configuration.Template.Name;
            set
            {
                Configuration.Template.Name = value;
                this.RaisePropertyChanged(nameof(TemplateName));
            }
        }

        public virtual bool AllowEditElements => true;

        public ModelElementConfiguration Configuration { get; set; }

        public ModelElementViewModel(ModelElementConfiguration elementConfig)
        {
            Configuration = elementConfig;

            CreateElementCommand = ReactiveCommand.CreateFromTask(CreateElement);
            RemoveElementCommand = ReactiveCommand.CreateFromTask(RemoveElement);
        }

        public ReactiveCommand<Unit, Unit> CreateElementCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveElementCommand { get; }

        private Task RemoveElement()
        {
            return Task.Run(() =>
            {
                if (Parent != null)
                   Parent.Elements.Remove(this);
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

        [Browsable(false)]
        public ObservableCollection<IModelElement> Elements { get; set; } = new ObservableCollection<IModelElement>();
        IList<IModelElement> IModelElement.Elements => Elements;

        [Browsable(false)]
        public ObservableCollection<IModelAttribute> Attributes { get; set; } = new ObservableCollection<IModelAttribute>();
        IList<IModelAttribute> IModelElement.Attributes => Attributes;

        [Browsable(false)]
        public IDataPoint? DataPoint { get; set; }

        [Browsable(false)]
        public IModelElement? Parent { get; set; }

        public void AddElement(IModelElement childElement)
        {
            Elements.Add(childElement);
        }
    }

    public class ModelAttributeViewModel : ViewModelBase, IModelAttribute
    {
        public ModelAttributeViewModel()
        {

        }

        public ModelAttributeViewModel(ModelAttributeTemplateConfiguration elementAttributeTemplate) : this()
        {
            Name = elementAttributeTemplate.Name;
            AttributeType = elementAttributeTemplate.AttributeType;
            Type = elementAttributeTemplate.Type;
        }

        public TypeCode Type { get; set; }

        public ModelAttributeType AttributeType { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}