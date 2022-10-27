using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.Adapter
{
    public class AdapterStopCommand : Command
    {
    }

    public class AdapterStopCommandHandler : CommandHandlerAsync<AdapterStopCommand>
    {
        public AdapterStopCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(AdapterStopCommand command)
        {
            await Task.CompletedTask;
        }
    }
}