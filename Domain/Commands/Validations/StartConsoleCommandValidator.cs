using FluentValidation;

namespace Domain.Commands.Validations;

public class StartConsoleCommandValidator : AbstractValidator<StartConsoleCommand>
{
    public StartConsoleCommandValidator()
    {
        RuleFor(command => command.Arguments).NotNull().NotEmpty();
    }
}