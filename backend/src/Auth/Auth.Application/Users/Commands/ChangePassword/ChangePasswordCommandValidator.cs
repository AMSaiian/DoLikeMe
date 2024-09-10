using Auth.Application.Common.Constants;
using FluentValidation;

namespace Auth.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(command => command.OldPassword)
            .NotEqual(command => command.NewPassword)
            .WithMessage(ErrorMessagesConstants.NoChangesProvided);

        RuleFor(command => command.OldPassword)
            .NotEmpty()
            .MinimumLength(ValidationConstants.UserPasswordMinimumLength)
            .MaximumLength(ValidationConstants.UserPasswordMaximumLength)
            .Matches(ValidationConstants.UserPasswordRegex);

        RuleFor(command => command.NewPassword)
            .Equal(command => command.NewPasswordConfirmation);

        RuleFor(command => command.NewPassword)
            .NotEmpty()
            .MinimumLength(ValidationConstants.UserPasswordMinimumLength)
            .MaximumLength(ValidationConstants.UserPasswordMaximumLength)
            .Matches(ValidationConstants.UserPasswordRegex);
    }
}
