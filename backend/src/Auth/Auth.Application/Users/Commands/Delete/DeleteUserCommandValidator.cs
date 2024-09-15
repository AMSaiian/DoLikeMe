using Auth.Application.Common.Constants;
using FluentValidation;

namespace Auth.Application.Users.Commands.Delete;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Password)
            .Equal(command => command.ConfirmPassword);

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(ValidationConstants.UserPasswordMinimumLength)
            .MaximumLength(ValidationConstants.UserPasswordMaximumLength)
            .Matches(ValidationConstants.UserPasswordRegex);
    }
}
