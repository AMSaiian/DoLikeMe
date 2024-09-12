using FluentValidation;

namespace Taskio.Application.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.AuthId)
            .NotEmpty();
    }
}
