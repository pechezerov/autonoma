namespace Autonoma.API.Commands.Validation
{
    public interface IValidatedCommand
    {
        ValidationResult ValidationResult { get; set; }
    }
}
