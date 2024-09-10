using Auth.Application.Common.Constants;
using FluentValidation;

namespace Auth.Application.Users.Commands.Register;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(ValidationConstants.UserNameLength)
            .Matches(ValidationConstants.UserNameRegex);

        RuleFor(command => command.Email)
            .NotEmpty()
            .MaximumLength(ValidationConstants.UserEmailLength)
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .MinimumLength(ValidationConstants.UserPasswordMinimumLength)
            .MaximumLength(ValidationConstants.UserPasswordMaximumLength)
            .Matches(ValidationConstants.UserPasswordRegex);
    }
}
