using Autonoma.Configuration;
using Autonoma.UI.Configuration.Abstractions;
using Autonoma.UI.Configuration.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
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

            return project;
        }

        public string SerializeProject(IProject value)
        {
            throw new NotImplementedException();
        }
    }
}
