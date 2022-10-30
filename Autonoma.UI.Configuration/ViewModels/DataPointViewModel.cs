using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System;
using System.Collections.Generic;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class DataPointViewModel : ViewModelBase, IDataPoint
    {
        public DataPointViewModel(DataPointConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
            Unit = configuration.Unit;
            Mapping = configuration.Settings;
        }

        public bool AllowEditElements => false;

        public string Name { get; set; }

        public TypeCode Type { get; set; } = TypeCode.Int32;

        public string Unit { get; set; } = "";

        public string Mapping { get; set; } = "";

        public IModelElement? Parent { get; set; }

        public IList<IModelElement> Elements => Array.Empty<IModelElement>();

        public object? Settings { get; set; }

        public bool Invalid { get; set; }

        public void AddElement(IModelElement element)
        {
            
        }
    }
}