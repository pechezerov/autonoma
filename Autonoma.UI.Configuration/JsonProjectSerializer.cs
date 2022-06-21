﻿using Autonoma.Configuration;
using Autonoma.Domain.Entities;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
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
            var templates = Context.ModelTemplates
                .ToList();
            var flatModel = Context.ModelElements
                .Include(m => m.Attributes)
                .ToList();

            var treeModel = GenerateModelTree(flatModel);

            foreach (var rootElementConfig in treeModel)
            {
                project.Topology.AddElement(BuildElementViewModel(rootElementConfig, null));
            }
        }

        private ModelElementViewModel BuildElementViewModel(ModelElementConfiguration elementConfig, ModelElementViewModel? parentElementViewModel)
        {
            var element = new ModelElementViewModel
            {
                Name = elementConfig.Name
            };

            foreach (var childConfig in elementConfig.Elements)
            {
                var childElement = BuildElementViewModel(childConfig, element);
                element.AddElement(childElement);
            }

            return element;
        }

        public IEnumerable<ModelElementConfiguration> GenerateModelTree(
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

        public string SerializeProject(IProject value)
        {
            throw new NotImplementedException();
        }
    }
}