using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Routing;
using Autonoma.API.Infrastructure;
using Autonoma.Model.Akka.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Autonoma.Model.Akka.Services
{
    /// <summary>
    /// <see cref="IHostedService"/> that runs and manages <see cref="ActorSystem"/> in background of application.
    /// </summary>
    public sealed class AkkaService : IHostedService
    {
        private ActorSystem _system;
        private readonly IServiceProvider _serviceProvider;

        private readonly IHostApplicationLifetime _applicationLifetime;

        public AkkaService(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime)
        {
            _serviceProvider = serviceProvider;
            _applicationLifetime = appLifetime;
            _system = ActorSystem.Create("System");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var uow = _serviceProvider.GetService<IUnitOfWork>();
            
            var bootstrap = BootstrapSetup.Create();
            // enable DI support inside this ActorSystem, if needed
            var diSetup = DependencyResolverSetup.Create(_serviceProvider);

            // merge this setup (and any others) together into ActorSystemSetup
            var actorSystemSetup = bootstrap.And(diSetup);
            _system = ActorSystem.Create("system", actorSystemSetup);

            // instantiate actors
            foreach (var adapterPrototype in uow.AdapterRepository
                .AllIncludeAsQueryable(a => a.DataPoints, a => a.AdapterType))
            {
                var adapter = _system.ActorOf(
                   Props.Create(() => new DataAdapterActor(adapterPrototype)), adapterPrototype.Name);

                foreach (var dataPointPrototype in adapterPrototype.DataPoints)
                {
                    var dataPoint = _system.ActorOf(
                       Props.Create(() => new DataPointActor(dataPointPrototype)), dataPointPrototype.Name);
                }
            }

            // add a continuation task that will guarantee shutdown of application if ActorSystem terminates
            _system.WhenTerminated.ContinueWith(tr => {
                _applicationLifetime.StopApplication();
            });

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // strictly speaking this may not be necessary - terminating the ActorSystem would also work
            // but this call guarantees that the shutdown of the cluster is graceful regardless
            await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}
