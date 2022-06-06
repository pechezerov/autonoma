using Autofac;
using Autonoma.API.Commands;
using Autonoma.API.Commands.Validation;
using Autonoma.API.Main.Infrastructure;
using Autonoma.API.Queries;
using Autonoma.Communication.Abstractions;
using Autonoma.Communication.Hosting;
using Autonoma.Communication.Hosting.Local;
using Microsoft.AspNetCore.SignalR;
using System.Reflection;
using Autofac.Integration.WebApi;
using Autofac.Integration.SignalR;
using Microsoft.Extensions.Hosting;

namespace Autonoma.API
{
    internal class BusinessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            // Services
            builder.RegisterType<RouterService>()
                .As<IRouterService>()
                .As<IHostedService>()
                .SingleInstance();
            builder.RegisterType<DataPointHostService>()
                .As<IDataPointService>()
                .SingleInstance();

            // Infrastructure
            builder
               .RegisterType<DatabaseInitializer>()
               .As<IStartable>()
               .SingleInstance();
            builder
               .RegisterType<DataPointServiceStartupInitializer>()
               .As<IStartable>()
               .SingleInstance();

            // CQRS
            builder.RegisterGeneric(typeof(QueryHandler<,>)).As(typeof(IQueryHandler<,>)).InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(IQueryHandlerAsync<,>));
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .AsClosedTypesOf(typeof(ICommandHandlerAsync<>));
            /*builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();*/
        }
    }
}