using Autonoma.API.Main.Infrastructure;
using Autonoma.Domain;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Autonoma.UI.Operation
{
    public class CommunicationService
    {
        private readonly HubConnection _connection;

        public CommunicationService()
        {
            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/Data")
                .WithAutomaticReconnect()
                .AddNewtonsoftJsonProtocol()
                .Build();

            _connection.On<List<DataPointInfo>>(nameof(IDataRouter.SendUpdates), WhenUpdatesReceived);

            _connection.StartAsync();
            _connection.InvokeAsync("Subscribe", new List<int> { 1, 3 });
        }

        private void WhenUpdatesReceived(List<DataPointInfo> obj)
        {
            OnUpdatesReceived?.Invoke(this, obj);
        }

        public void Subscribe(List<int> dataPointIds)
        {
            _connection.SendAsync(nameof(Subscribe), dataPointIds);
        }

        public EventHandler<List<DataPointInfo>>? OnUpdatesReceived;
    }
}