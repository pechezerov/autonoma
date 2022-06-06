using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using Autonoma.API.Main.Infrastructure;
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
            var adapters = _uow.AdapterRepository.AllIncludeAsQueryable(a => a.AdapterTypeId);
            foreach (var adapter in adapters)
            {
                throw new NotImplementedException();
            }

            await Task.CompletedTask;
        }
    }
}