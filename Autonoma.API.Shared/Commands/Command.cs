using Autonoma.API.Commands.Validation;
using ValidationResult = Autonoma.API.Commands.Validation.ValidationResult;

namespace Autonoma.API.Commands
{
    public class Command : ICommand
    {

    }

    public abstract class ValidatedCommand : Command, IValidatedCommand
    {
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();
    }
}
