using Autonoma.API.Infrastructure;

namespace Autonoma.API.Commands
{
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
            where TCommand : ICommand
    {
        protected readonly IUnitOfWork _uow;

        public CommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public abstract void Execute(TCommand command);
    }
}
