using FluentValidation;
using Task.io.Domain.Entities;

namespace Task.io.Application.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.AuthId)
            .NotEmpty();
    }
}
