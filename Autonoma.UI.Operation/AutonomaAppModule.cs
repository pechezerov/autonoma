#nullable enable
using Autofac;
using Autonoma.UI.Shapes.Electric;
using Core2D.Model.Editor;
using System.Reflection;
using Module = Autofac.Module;

namespace Autonoma.UI.Operation
{
    public class AutonomaAppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AutonomaViewModelFactory>().As<IAutonomaViewModelFactory>().InstancePerLifetimeScope();

            // ViewModels

            builder.RegisterAssemblyTypes(typeof(AutonomaViewModelFactory).GetTypeInfo().Assembly)
                .PublicOnly()
                .Where(t =>
                {
                    if (t.Namespace is null)
                    {
                        return false;
                    }
                    if (t.Namespace.StartsWith("Autonoma.UI.Shapes.Electric")
                        && t.Name.EndsWith("ViewModel"))
                    {
                        return true;
                    }
                    return false;
                })
                .AsSelf()
                .InstancePerDependency();

            // Editor

            builder.RegisterAssemblyTypes(typeof(AutonomaViewModelFactory).GetTypeInfo().Assembly)
                .PublicOnly()
                .Where(t => t.Namespace is not null && t.Namespace.StartsWith("Autonoma.UI.Shapes.Electric"))
                .As<IEditorTool>()
                .AsSelf()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(AutonomaViewModelFactory).GetTypeInfo().Assembly)
                .PublicOnly()
                .Where(t => t.Namespace is not null && t.Namespace.StartsWith("Autonoma.UI.Shapes.Electric"))
                .As<IBounds>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
