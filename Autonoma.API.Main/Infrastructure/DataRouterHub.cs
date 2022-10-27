using Autonoma.Communication.Abstractions;
using Autonoma.Communication.Hosting;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Infrastructure
{
    public class DataRouterHub : Hub<IDataRouter>
    {
        private readonly IDataPointService _dataPointService;
        private readonly IRouterService _routerService;

        public DataRouterHub(IDataPointService dataPointService, IRouterService routerService)
        {
            _dataPointService = dataPointService;
            _routerService = routerService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task Subscribe(List<int> dataPointIds)
        {
            dataPointIds = dataPointIds.Distinct().ToList();
            await Read(dataPointIds);
            _routerService.SubscribeToDataPoints(Context.ConnectionId, dataPointIds);
        }

        public async Task Read(List<int> dataPointIds)
        {
            var dataPointValues = (await _dataPointService.GetDataPointValues(dataPointIds));
            await Clients.Caller.SendUpdates(dataPointValues);
        }
    }
}