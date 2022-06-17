using System;
using System.Collections.Generic;

namespace Autonoma.Domain.Entities
{
    public class ModelElementConfiguration : Entity
    {
        public virtual ModelElementTemplateConfiguration Template { get; set; } = null!;

        public int TemplateId { get; set; }

        public List<ModelElementConfiguration> Elements { get; set; }

        public virtual ModelElementConfiguration? ParentElement { get; set; }

        public int? ParentElementId { get; set; }

        public List<ModelAttributeConfiguration> Attributes { get; set; }

        public string Name { get; set; }

        public ModelElementConfiguration()
        {
            Elements = new List<ModelElementConfiguration>();
            Attributes = new List<ModelAttributeConfiguration>();
        }
    }

    public class ModelElementTemplateConfiguration : Entity
    {
        public virtual ModelElementTemplateConfiguration? BaseTemplate { get; set; }

        public int? BaseTemplateId { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public List<ModelAttributeTemplateConfiguration> Attributes { get; set; }

        public List<ModelElementConfiguration> Elements { get; set; }

        public ModelElementTemplateConfiguration()
        {
            Attributes = new List<ModelAttributeTemplateConfiguration>();
            Elements = new List<ModelElementConfiguration>();
        }
    }

    public class ModelAttributeTemplateConfiguration : Entity
    {
        public ModelAttributeType AttributeType { get; set; }

        public string Description { get; set; } = "";

        public string Name { get; set; } = "";

        public ModelElementTemplateConfiguration Template { get; set; } = null!;

        public int TemplateId { get; set; }

        public TypeCode Type { get; set; }

        public UOM UOM { get; set; }

        public List<ModelAttributeConfiguration> Attributes { get; set; }

        public ModelAttributeTemplateConfiguration()
        {
            Attributes = new List<ModelAttributeConfiguration>();
        }
    }

    public class ModelAttributeConfiguration : Entity
    {
        public ModelAttributeType AttributeType { get; set; }

        public ModelAttributeTemplateConfiguration Template { get; set; } = null!;

        public int TemplateId { get; set; }

        public ModelElementConfiguration Element { get; set; } = null!;

        public int ElementId { get; set; }

        public TypeCode Type { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public UOM UOM { get; set; }

        public ModelAttributeConfiguration()
        {
        }
    }

    public enum ModelAttributeType
    {
    }

    public enum UOM
    {
    }
}
