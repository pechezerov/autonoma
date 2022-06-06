using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Adapter
{
    public class AdapterStartCommand : Command
    {
    }

    public class AdapterStartCommandHandler : CommandHandlerAsync<AdapterStartCommand>
    {
        public AdapterStartCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(AdapterStartCommand command)
        {
            await Task.CompletedTask;
        }
    }
}