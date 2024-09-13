using FluentValidation;

namespace Taskio.Application.Users.Commands.Delete;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEqual(Guid.Empty);
    }
}
