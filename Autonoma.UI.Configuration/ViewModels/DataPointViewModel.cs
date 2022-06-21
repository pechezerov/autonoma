using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Presentation.ViewModels;
using System;

namespace Autonoma.UI.Configuration.ViewModels
{
    public class DataPointViewModel : ViewModelBase, IDataPoint
    {
        public DataPointViewModel(DataPointConfiguration configuration)
        {
            Name = configuration.Name;
            Type = configuration.Type;
            Unit = configuration.Unit;
            Mapping = configuration.Mapping;
        }

        public string Name { get; set; }

        public TypeCode Type { get; set; } = TypeCode.Int32;

        public string Unit { get; set; } = "";

        public string Mapping { get; set; } = "";

    }
}