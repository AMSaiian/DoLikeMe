using FluentValidation;

namespace Taskio.Application.Users.Commands.Create;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.AuthId)
            .NotEmpty();
    }
}
