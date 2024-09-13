using Auth.Application.Common.Constants;
using FluentValidation;

namespace Auth.Application.Users.Commands.UpdateProfileInfo;

public sealed class UpdateProfileInfoCommandValidator : AbstractValidator<UpdateProfileInfoCommand>
{
    public UpdateProfileInfoCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command)
            .Must(command => command.NewName is not null || command.NewEmail is not null)
            .WithMessage(ErrorMessagesConstants.NoChangesProvided);

        RuleFor(command => command.NewName)
            .NotEmpty()
            .When(command => command.NewName is not null)
            .MaximumLength(ValidationConstants.UserNameLength)
            .Matches(ValidationConstants.UserNameRegex);

        RuleFor(command => command.NewEmail)
            .NotEmpty()
            .When(command => command.NewEmail is not null)
            .MaximumLength(ValidationConstants.UserEmailLength)
            .EmailAddress();
    }
}
