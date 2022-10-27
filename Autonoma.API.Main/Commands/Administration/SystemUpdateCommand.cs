using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Administration
{
    public class SystemUpdateCommand : Command
    {
    }

    public class SystemUpdateCommandHandler : CommandHandlerAsync<SystemUpdateCommand>
    {
        public SystemUpdateCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(SystemUpdateCommand command)
        {
            throw new NotImplementedException();

            var adapters = _uow.AdapterRepository.AllIncludeAsQueryable(a => a.AdapterTypeId);
            foreach (var adapter in adapters)
            {
            }

            await Task.CompletedTask;
        }
    }
}