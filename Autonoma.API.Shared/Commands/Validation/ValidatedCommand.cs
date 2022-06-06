namespace Autonoma.API.Commands.Validation
{
    public abstract class ValidatedCommand : Command, IValidatedCommand
    {
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();
    }
}
