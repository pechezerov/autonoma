using Autonoma.Communication.Abstractions;
using Autonoma.Communication.Hosting;
using Autonoma.Domain;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Autonoma.API.Main.Infrastructure
{
    public class RouterService : HostedService, IRouterService
    {
        private readonly ILogger<RouterService> _logger;
        private readonly RouterSettings _settings;
        private readonly BufferBlock<DataPointInfo> _queue;
        private readonly IHubContext<DataRouterHub, IDataRouter> _dataHub;

        private Dictionary<string, HashSet<int>> dataPointSubscriptionsTable = new ();

        public RouterService(IHubContext<DataRouterHub, IDataRouter> dataHub, IOptions<RouterSettings> settings, ILogger<RouterService> logger)
        {
            _dataHub = dataHub;
            _settings = settings.Value;
            _logger = logger;
            _queue = new BufferBlock<DataPointInfo>();
        }

        public async Task Enqueue(DataPointInfo item)
        {
            await _queue.SendAsync(item);
        }

        public async Task Enqueue(IEnumerable<DataPointInfo> items)
        {
            foreach (DataPointInfo item in items)
            {
                await _queue.SendAsync(item);
            }
        }

        public void SubscribeToDataPoints(string connectionId, List<int> dataPointIds)
        {
            if (dataPointSubscriptionsTable.ContainsKey(connectionId))
                dataPointSubscriptionsTable.Remove(connectionId);

            HashSet<int> subscriptions = new();
            foreach (var dataPointId in dataPointIds)
                subscriptions.Add(dataPointId);
            dataPointSubscriptionsTable.Add(connectionId, subscriptions);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await WriteDataStep();
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error on handling queue");
                }
            }
        }

        protected int GetFlushTimeoutMs()
        {
            return _settings.FlushIntervalMs ?? 1000;
        }

        protected virtual async Task WriteDataStep()
        {
            List<DataPointInfo> list = new List<DataPointInfo>();
            CancellationTokenSource cts = new CancellationTokenSource(GetFlushTimeoutMs());
            while (!cts.IsCancellationRequested)
            {
                try
                {
                    list.Add(await _queue.ReceiveAsync(cts.Token));
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            if (list.Any())
            {
                foreach (var clientSubscription in dataPointSubscriptionsTable)
                {
                    List<DataPointInfo> clientUpdates = new();
                    foreach (var update in list)
                    {
                        if (clientSubscription.Value.Contains(update.DataPointId))
                            clientUpdates.Add(update);
                    }

                    if (clientUpdates.Any())
                    {
                        await _dataHub.Clients.Client(clientSubscription.Key)
                            .SendUpdates(clientUpdates);
                    }
                }
            }
        }
    }
}