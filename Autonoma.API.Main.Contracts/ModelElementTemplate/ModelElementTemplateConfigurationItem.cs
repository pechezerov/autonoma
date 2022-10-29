using Autonoma.Domain.Entities;
using System.Collections.Generic;

namespace Autonoma.API.Main.Contracts.ModelElementTemplate
{
    public class ModelElementTemplateConfigurationItem
    {
        public int? BaseTemplateId { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public List<ModelAttributeTemplateConfigurationItem> Attributes { get; set; }
    }

    public class ModelAttributeTemplateConfigurationItem
    {
    }
}
