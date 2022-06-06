using System.Threading.Tasks;

namespace Autonoma.API.Commands.Validation.Async
{
    public class ValidationDecoratorAsync<TCommand> : ICommandHandlerAsync<TCommand> where TCommand : ICommand, IValidatedCommand
    {
        private readonly ICommandHandlerAsync<TCommand> _decoratedCommandHandler;
        private readonly IValidatorAsync<TCommand> _validator;

        public ValidationDecoratorAsync(ICommandHandlerAsync<TCommand> decoratedCommandHandler, IValidatorAsync<TCommand> validator)
        {
            _decoratedCommandHandler = decoratedCommandHandler;
            _validator = validator;
        }

        public async Task ExecuteAsync(TCommand command)
        {
            await _validator.ValidateAsync(command);

            if (command.ValidationResult.IsValid)
            {
                await _decoratedCommandHandler.ExecuteAsync(command);
            }
        }
    }
}
