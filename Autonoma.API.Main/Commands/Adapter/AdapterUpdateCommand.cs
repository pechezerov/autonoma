using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Contracts.Adapter;
using Autonoma.Domain.Entities;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Adapter
{
    public class AdapterUpdateCommand : Command
    {
        public AdapterConfigurationItem Adapter { get; set; }

        public AdapterUpdateCommand(AdapterConfigurationItem adapter)
        {
            Adapter = adapter;
        }
    }

    public class AdapterUpdateCommandHandler : CommandHandlerAsync<AdapterUpdateCommand>
    {
        public AdapterUpdateCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(AdapterUpdateCommand command)
        {
            var adapterInfo = command.Adapter;

            var adapter = new AdapterConfiguration
            {
                Id = adapterInfo.Id,
                AdapterTypeId = adapterInfo.AdapterTypeId,
                Name = adapterInfo.Name,
                Address = adapterInfo.Address,
                IpAddress = adapterInfo.IpAddress,
                Port = adapterInfo.Port
            };

            _uow.AdapterRepository.Update(adapter);
            await _uow.CommitAsync();
        }
    }
}