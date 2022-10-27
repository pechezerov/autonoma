using Autonoma.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autonoma.Domain.Entities
{
    public enum ModelAttributeType
    {
        None,
        Setting
    }

    public enum UOM
    {
    }

    public class ModelAttributeConfiguration : Entity
    {
        public ModelAttributeType AttributeType { get; set; }

        public string Description { get; set; } = "";

        public ModelElementConfiguration Element { get; set; } = null!;

        public int ElementId { get; set; }

        public string Name { get; set; } = "";

        public ModelAttributeTemplateConfiguration Template { get; set; } = null!;

        public int TemplateId { get; set; }

        public TypeCode Type { get; set; }

        public UOM UOM { get; set; }

        public ModelAttributeConfiguration()
        {
        }
    }

    public class ModelAttributeTemplateConfiguration : Entity
    {
        public List<ModelAttributeConfiguration> Attributes { get; set; }

        public ModelAttributeType AttributeType { get; set; }

        public int BaseTemplateId { get; set; }

        public string Description { get; set; } = "";

        public ModelElementTemplateConfiguration ElementTemplate { get; set; } = null!;

        public string Name { get; set; } = "";
        public TypeCode Type { get; set; }

        public UOM UOM { get; set; }

        public ModelAttributeTemplateConfiguration()
        {
            Attributes = new List<ModelAttributeConfiguration>();
        }

        public ModelAttributeTemplateConfiguration(ModelAttributeTemplateConfiguration configuration, ModelElementTemplateConfiguration owner) : this()
        {
            var data = JsonConvert.SerializeObject(configuration);
            JsonConvert.PopulateObject(data, this);
            ElementTemplate = owner;
        }
    }

    public class ModelElementConfiguration : Entity
    {
        public List<ModelAttributeConfiguration> Attributes { get; set; }

        public DataPointConfiguration? DataPoint { get; set; }

        public int? DataPointId { get; set; }

        public List<ModelElementConfiguration> Elements { get; set; }

        public string Name { get; set; }

        public virtual ModelElementConfiguration? ParentElement { get; set; }

        public int? ParentElementId { get; set; }

        public virtual ModelElementTemplateConfiguration Template { get; set; } = ModelElementTemplateConfiguration.Default;

        public int TemplateId { get; set; }

        public string Path
        {
            get { return ParentElement == null ? Name : $"{ParentElement.Path}/{Name}"; }
        }

        public ModelElementConfiguration()
        {
            Name = "";
            Elements = new List<ModelElementConfiguration>();
            Attributes = new List<ModelAttributeConfiguration>();
        }

        public static ModelElementConfiguration CreateNew()
        {
            return new ModelElementConfiguration()
            {
                Name = "New",
                Template = ModelElementTemplateConfiguration.Default
            };
        }

        public static IEnumerable<ModelElementConfiguration> GenerateModelTree(
            IEnumerable<ModelElementConfiguration> items,
            ModelElementConfiguration? localRoot = null)
        {
            var localRootElements = items.Where(c => c.ParentElementId == localRoot?.Id)
                .ToList();
            foreach (var localRootChild in localRootElements)
            {
                localRoot?.Elements.Add(localRootChild);
                localRootChild.ParentElement = localRoot;
                GenerateModelTree(items, localRootChild);
            }

            return localRootElements;
        }
    }

    public class ModelElementTemplateConfiguration : Entity
    {
        public static ModelElementTemplateConfiguration Default { get; }
            = new ModelElementTemplateConfiguration()
            {
                Name = "MeasuredValueDouble",
                Id = Globals.MeasuredValueIntModelTemplateId,
            };

        public List<ModelAttributeTemplateConfiguration> Attributes { get; set; }

        public virtual ModelElementTemplateConfiguration? BaseTemplate { get; set; }

        public int? BaseTemplateId { get; set; }

        public string Description { get; set; } = "";

        public List<ModelElementConfiguration> Elements { get; set; }

        public string Name { get; set; } = "";
        public ModelElementTemplateConfiguration()
        {
            Attributes = new List<ModelAttributeTemplateConfiguration>();
            Elements = new List<ModelElementConfiguration>();
        }

        public void AddAttribute(ModelAttributeTemplateConfiguration configuration)
        {
            Attributes.Add(
                new ModelAttributeTemplateConfiguration(configuration, this));
        }

        public void AddAttribute(string name, TypeCode type, ModelAttributeType attributeType = ModelAttributeType.None, string description = "")
        {
            Attributes.Add(
                new ModelAttributeTemplateConfiguration
                {
                    Name = name,
                    Type = type,
                    AttributeType = attributeType,
                    Description = description
                });
        }

        public IEnumerable<ModelAttributeTemplateConfiguration> GetModelAttributes()
        {
            return Attributes.Concat(BaseTemplate?.GetModelAttributes() ?? Enumerable.Empty<ModelAttributeTemplateConfiguration>());
        }
    }
}
