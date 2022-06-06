using Autonoma.API.Commands;
using Autonoma.API.Infrastructure;
using System.Threading.Tasks;

namespace Autonoma.API.Main.Commands.DataPoint
{
    public class DataPointDeleteCommand : Command
    {
        public int Id { get; internal set; }
    }

    public class DataPointDeleteCommandHandler : CommandHandlerAsync<DataPointDeleteCommand>
    {
        public DataPointDeleteCommandHandler(IUnitOfWork uow) : base(uow)
        {
        }

        public override async Task ExecuteAsync(DataPointDeleteCommand command)
        {
            await Task.CompletedTask;
        }
    }
}