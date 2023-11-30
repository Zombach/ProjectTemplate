using FluentValidation;

namespace Domain.Commands.Validations;

public class StartConsoleCommandValidator : BaseValidator<StartConsoleCommand>
{
    public StartConsoleCommandValidator()
    {
        RuleFor(command => command.Arguments).NotNull();
    }
}