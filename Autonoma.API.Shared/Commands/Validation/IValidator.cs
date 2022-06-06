namespace Autonoma.API.Commands.Validation
{
    public interface IValidator<TCommand> where TCommand : ICommand
    {
        void Validate(TCommand command);
    }
}
