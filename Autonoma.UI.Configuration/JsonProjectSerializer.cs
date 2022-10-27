using Autonoma.Configuration;
using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Autonoma.UI.Configuration
{
    internal class SqliteProjectSerializer : IProjectSerializer
    {
        public SqliteProjectSerializer(IConfiguration config, ConfigurationContext context)
        {
            Config = config;
            Context = context;
        }

        public IConfiguration Config { get; }

        public ConfigurationContext Context { get; }

        public IProject DeserializeProject<T>(string path) where T : IProject
        {
            var project = new ProjectViewModel();
            project.FilePath = path;

            FillTechnology(project);
            FillTopology(project);

            return project;
        }

        private void FillTechnology(ProjectViewModel project)
        {
            foreach (var adapterConfig in Context.Adapters
                .Include(a => a.DataPoints)
                .Include(a => a.AdapterType))
            {
                IRouter? adapterOwner = null;
                adapterOwner = project.Technology.Routers?.FirstOrDefault(r => r.AdapterTypeId == adapterConfig.AdapterTypeId);
                if (adapterOwner == null)
                {
                    adapterOwner = new RouterViewModel()
                    {
                        AdapterTypeId = adapterConfig.AdapterTypeId,
                        Name = adapterConfig.Name
                    };
                }

                var adapter = new AdapterViewModel(adapterConfig);
                adapterOwner.Adapters.Add(adapter);

                if (!project.Technology.Routers!.Contains(adapterOwner))
                    project.Technology.Routers.Add(adapterOwner);
            }
        }

        private void FillTopology(ProjectViewModel project)
        {
            var templatesDict = Context.ModelTemplates
                .Include(t => t.Attributes)
                .Include(t => t.BaseTemplate!.Attributes)
                .ToDictionary(t => t.Id, t => t);

            var flatModel = Context.ModelElements
                .Include(e => e.Attributes)
                .ToList();

            foreach (var modelElement in flatModel)
                modelElement.Template = templatesDict[modelElement.TemplateId];

            var treeModel = ModelElementConfiguration.GenerateModelTree(flatModel, null);

            foreach (var rootElementConfig in treeModel)
                project.Topology.AddElement(BuildElementViewModel(rootElementConfig, null));
        }

        private ModelElementViewModel BuildElementViewModel(ModelElementConfiguration elementConfig, ModelElementViewModel? parentElementViewModel)
        {
            var element = new SimpleModelElementViewModel(elementConfig);
            element.Parent = parentElementViewModel;

            foreach (var elementAttributeTemplate in elementConfig.Template.GetModelAttributes())
                element.Attributes.Add(new ModelAttributeViewModel(elementAttributeTemplate));

            foreach (var childConfig in elementConfig.Elements)
            {
                var childElement = BuildElementViewModel(childConfig, element);
                element.AddElement(childElement);
            }

            return element;
        }

        public string SerializeProject(IProject value)
        {
            throw new NotImplementedException();
        }
    }
}
