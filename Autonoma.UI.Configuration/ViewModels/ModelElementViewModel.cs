﻿using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class ModelElementViewModel : ViewModelBase, IModelElement
    {
        public string Name { get; set; } = "";

        [Browsable(false)]
        public ObservableCollection<IModelElement> Elements { get; set; } = new ObservableCollection<IModelElement>();

        public IDataPoint? DataPoint { get; set; }

        public void AddElement(IModelElement childElement)
        {
            Elements.Add(childElement);
        }
    }
}