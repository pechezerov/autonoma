namespace Autonoma.API.Commands.Validation
{
    public class ValidationDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand, IValidatedCommand
    {
        private readonly ICommandHandler<TCommand> _decoratedCommandHandler;
        private readonly IValidator<TCommand> _validator;

        public ValidationDecorator(ICommandHandler<TCommand> decoratedCommandHandler, IValidator<TCommand> validator)
        {
            _decoratedCommandHandler = decoratedCommandHandler;
            _validator = validator;
        }

        public void Execute(TCommand command)
        {
            _validator.Validate(command);

            if (command.ValidationResult.IsValid)
            {
                _decoratedCommandHandler.Execute(command);
            }
        }
    }
}
