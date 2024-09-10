using Auth.Application.Common.Constants;
using FluentValidation;

namespace Auth.Application.Users.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(command => command.Identifier)
            .NotEmpty()
            .Matches(ValidationConstants.UserNameRegex);

        RuleFor(command => command.Password)
            .NotEmpty()
            .Matches(ValidationConstants.UserPasswordRegex);
    }
}

