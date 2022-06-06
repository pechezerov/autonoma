using System.Threading.Tasks;

namespace Autonoma.API.Commands
{
    public interface ICommandHandlerAsync<TCommand> where TCommand : ICommand
    {
        Task ExecuteAsync(TCommand command);
    }
}
