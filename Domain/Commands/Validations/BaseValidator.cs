using FluentValidation;

namespace Domain.Commands.Validations;

public abstract class BaseValidator<T> : AbstractValidator<T>;