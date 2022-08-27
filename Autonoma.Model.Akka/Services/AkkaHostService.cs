using Akka.Actor;
using Akka.DependencyInjection;
using Autonoma.API.Infrastructure;
using Autonoma.Communication.Abstractions;
using Autonoma.Communication.Hosting;
using Autonoma.Domain;
using Autonoma.Domain.Entities;
using Autonoma.Model.Akka.Actors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace Autonoma.Model.Akka.Services
{
    /// <summary>
    /// <see cref="IHostedService"/> that runs and manages <see cref="ActorSystem"/> in background of application.
    /// </summary>
    public sealed class AkkaHostService : IHostedService, IDataPointService
    {
        private ActorSystem _system;
        private readonly IRouterService _routerService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _applicationLifetime;

        private ConcurrentDictionary<int, DataValue> _valuesDict = new ConcurrentDictionary<int, DataValue>();
        private ConcurrentDictionary<int, IActorRef> _actorsIdDict = new ConcurrentDictionary<int, IActorRef>();
        private ConcurrentDictionary<string, IActorRef> _actorsPathDict = new ConcurrentDictionary<string, IActorRef>();

        public AkkaHostService(IServiceProvider serviceProvider, IHostApplicationLifetime appLifetime, IRouterService routerService)
        {
            _routerService = routerService;
            _serviceProvider = serviceProvider;
            _applicationLifetime = appLifetime;
            _system = ActorSystem.Create("System");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var uow = _serviceProvider.GetRequiredService<IUnitOfWork>();

            var bootstrap = BootstrapSetup.Create();
            // enable DI support inside this ActorSystem, if needed
            var diSetup = DependencyResolverSetup.Create(_serviceProvider);

            // merge this setup (and any others) together into ActorSystemSetup
            var actorSystemSetup = bootstrap.And(diSetup);
            _system = ActorSystem.Create("system", actorSystemSetup);

            // associate elements with their templates
            var templatesDict = uow.ModelTemplateRepository
                .AllIncludeAsQueryable(mt => mt.Attributes, mt => mt.BaseTemplate!.Attributes)
                .ToDictionary(t => t.Id, t => t);
            var flatModel = uow.ModelRepository
                .AllIncludeAsQueryable(m => m.Attributes)
                .ToList();

            var rootElements = ModelElementConfiguration.GenerateModelTree(flatModel, null);

            // assign templates
            foreach (var elementPrototype in flatModel)
                elementPrototype.Template = templatesDict[elementPrototype.TemplateId];

            // instantiate actors
            foreach (var rootElementPrototype in rootElements)
            {
                Register(rootElementPrototype);
            }

            // add a continuation task that will guarantee shutdown of application if ActorSystem terminates
            _system.WhenTerminated.ContinueWith(tr =>
            {
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

        public Task<DataPointInfo?> GetDataPointValue(int id)
        {
            return Task.Run(() => GetDataPointValueInternal(id));
        }

        private DataPointInfo? GetDataPointValueInternal(int id)
        {
            if (_valuesDict.TryGetValue(id, out var v))
                return new DataPointInfo(id, v);
            return null;
        }

        public Task<List<DataPointInfo>> GetDataPointValues(List<int>? ids)
        {
            if (ids != null)
            {
                return Task.Run(() => ids
                    .Select(id => GetDataPointValueInternal(id))
                    .Where(dpi => dpi != null)
                    .Cast<DataPointInfo>()
                    .ToList());
            }
            else
            {
                return Task.FromResult(new List<DataPointInfo>());
            }
        }

        public async Task UpdateDataPoint(int id, DataValue value)
        {
            if (_actorsIdDict.TryGetValue(id, out var target))
                target.Tell(value);

            await _routerService.Enqueue(new DataPointInfo(id, value));
        }

        public async Task UpdateDataPoints(IEnumerable<(int id, DataValue value)> updates)
        {
            foreach (var update in updates)
                await UpdateDataPoint(update.id, update.value);
        }

        public void Register(ModelElementConfiguration configuration)
        {
            var actor = _system.ActorOf(Props.Create(() => new DataPointActor(configuration, this)), configuration.Name);
            _actorsIdDict.TryAdd(configuration.Id, actor);
            _actorsPathDict.TryAdd(configuration.Path, actor);

            foreach (var childElement in configuration.Elements)
                Register(childElement);
        }

        public void ReceiveUpdateCallback(int id, DataValue value)
        {
            _valuesDict.AddOrUpdate(id, value,
                (i, v) =>
                {
                    return value;
                });
        }

        public void CreateSystemDataPoint(DataPointConfiguration config)
        {
        }

        public void RemoveDataPoint(int id)
        {
        }
    }
}
