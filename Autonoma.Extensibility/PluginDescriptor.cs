using System;
using System.Reflection;

namespace Autonoma.Extensibility
{
    internal class PluginDescriptor
    {
        public PluginLoadContext Context { get; }
        public Assembly Assembly { get; }
        public Type Type { get; }

        internal PluginDescriptor(PluginLoadContext context, Assembly assembly, Type pluginType)
        {
            Context = context;
            Type = pluginType;
            Assembly = assembly;
        }
    }

    internal class AssemblyDescriptor
    {
        public PluginLoadContext Context { get; }
        public Assembly Assembly { get; }

        internal AssemblyDescriptor(PluginLoadContext context, Assembly assembly)
        {
            Context = context;
            Assembly = assembly;
        }
    }
}
