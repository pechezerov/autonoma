using System.Threading.Tasks;

namespace Autonoma.API.Commands.Validation.Async
{
    public interface IValidatorAsync<TCommand> where TCommand : ICommand
    {
        Task ValidateAsync(TCommand command);
    }
}
