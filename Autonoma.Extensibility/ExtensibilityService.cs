using Autonoma.Extensibility.Shared.Abstractions;
using Autonoma.Extensibility.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Autonoma.Extensibility
{
    public class ExtensibilityService : IExtensibilityService
    {
        private static readonly string folderToScan;

        private readonly Dictionary<string, AssemblyDescriptor> loadedAssemblies = new Dictionary<string, AssemblyDescriptor>();
        private Dictionary<string, PluginDescriptor> loadedPlugins = new Dictionary<string, PluginDescriptor>();

        static ExtensibilityService()
        {
            folderToScan = Path.Combine(AppContext.BaseDirectory, "Plugins");
            // Подготавливаем каталоги для размещения файлов, если они не существуют
            Directory.CreateDirectory(folderToScan);
        }

        public ExtensibilityService()
        {
            Initialize();
        }

        public List<PluginInfo> GetImplementations<T>()
        {
            return loadedPlugins
                .Where(p => typeof(T).IsAssignableFrom(p.Value.Type))
                .Select(p => new PluginInfo
                {
                    TypeName = p.Value.Type.FullName,
                    Description = p.Value.Type.GetCustomAttribute<DescriptionAttribute>()?.Description
                })
                .ToList();
        }

        public T GetInstance<T>(string pluginName)
        {
            var targetEntry = loadedPlugins[pluginName];
            if (targetEntry != null)
            {
                var targetType = targetEntry.Type;

                if (targetType != null)
                {
                    var createdInstance = CreatePlugin(targetType);
                    if (typeof(T).IsAssignableFrom(createdInstance.GetType()))
                        return (T)createdInstance;
                }
            }
            return default;
        }

        public void Initialize()
        {
            if (loadedAssemblies.Any())
            {
                foreach (var loadedAssembly in loadedAssemblies)
                {
                    loadedAssembly.Value.Context.Unload();
                }
            }

            loadedAssemblies.Clear();
            loadedPlugins.Clear();

            string[] pluginPaths = Directory.GetFiles(folderToScan)
                .Where(f => Path.GetExtension(f) == ".dll").ToArray();

            List<PluginDescriptor> result = new List<PluginDescriptor>();

            foreach (var pluginPath in pluginPaths)
            {
                var context = new PluginLoadContext(pluginPath);
                try
                {
                    Assembly assembly;
                    using (var stream = File.OpenRead(pluginPath))
                    {
                        string debug;
                        if (pluginPath.EndsWith(".dll") && File.Exists(debug = pluginPath[..^4] + ".pdb"))
                        {
                            using var debugStream = File.OpenRead(debug);
                            assembly = context.LoadFromStream(stream, debugStream);
                        }
                        else
                        {
                            assembly = context.LoadFromStream(stream);
                        }
                    }

                    var pluginTypes = assembly.GetExportedTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x)).ToList();
                    if (pluginTypes.Count == 0)
                    {
                        throw new Exception($"No class extending {nameof(IPlugin)} found in {pluginPath}");
                    }

                    loadedAssemblies[assembly.GetName().Name] = new AssemblyDescriptor(context, assembly);

                    foreach (var pluginType in pluginTypes)
                    {
                        result.Add(new PluginDescriptor(context, assembly, pluginType));
                    }
                }
                catch
                {
                }
            }

            loadedPlugins = result.ToDictionary(p => p.Type.FullName, p => p);
        }

        public Task Reload()
        {
            return Task.Run(() => Initialize());
        }

        public void Unload(string name)
        {
            if (loadedPlugins.ContainsKey(name))
            {
                var info = loadedPlugins[name];
                info.Context.Unload();
                loadedPlugins.Remove(name);
            }
        }

        private IPlugin CreatePlugin(Type targetType)
        {
            var activatedInstance = Activator.CreateInstance(targetType);
            if (activatedInstance != null)
                return activatedInstance as IPlugin;
            else return null;
        }
    }
}
