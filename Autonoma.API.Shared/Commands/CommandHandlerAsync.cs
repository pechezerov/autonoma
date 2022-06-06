using Autonoma.API.Infrastructure;
using System.Threading.Tasks;

namespace Autonoma.API.Commands
{
    public abstract class CommandHandlerAsync<TCommand> : ICommandHandlerAsync<TCommand>
            where TCommand : ICommand
    {
        protected readonly IUnitOfWork _uow;

        public CommandHandlerAsync(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public abstract Task ExecuteAsync(TCommand command);
    }
}
