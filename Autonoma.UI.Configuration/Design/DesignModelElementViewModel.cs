using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.ViewModels;
using System;

namespace Autonoma.UI.Configuration.Design
{
    internal class DesignModelElementViewModel : ModelElementViewModel
    {
        public DesignModelElementViewModel(int counter) : base(new ModelElementConfiguration { Name = $"Element{counter}" })
        {
            DataPoint = new DataPointViewModel(new DataPointConfiguration
            {
                Id = -1,
                Mapping = "params(1) params(2) [0..9]",
                Source = Domain.DataSource.Device,
                Type = TypeCode.Double,
                Unit = "kW",
            });

            Attributes.Add(new ModelAttributeViewModel { Name = "IntProperty", Value = "1" });
            Attributes.Add(new ModelAttributeViewModel { Name = "StringProperty", Value = "text" });
        }
    }
}