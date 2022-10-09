using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.API.Main.Queries.Adapter;
using Autonoma.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Adapter
{
    public class AdapterCreateCommand : Command
    {
        public AdapterConfigurationItem Adapter { get; set; }

        public int CreatedId { get; set; }
    }

    public class AdapterCreateCommandHandler : CommandHandlerAsync<AdapterCreateCommand>
    {
        public AdapterCreateCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(AdapterCreateCommand command)
        {
            var adapterInfo = command.Adapter;

            var adapter = new AdapterConfiguration
            {
                AdapterTypeId = adapterInfo.AdapterTypeId,
                Name = adapterInfo.Name,
                Address = adapterInfo.Address,
                IpAddress = adapterInfo.IpAddress,
                Port = adapterInfo.Port
            };
            // TODO: развернутые сведения о типе (AdapterType)

            _uow.AdapterRepository.Create(adapter);
            await _uow.CommitAsync();

            command.CreatedId = adapter.Id;
        }
    }
}