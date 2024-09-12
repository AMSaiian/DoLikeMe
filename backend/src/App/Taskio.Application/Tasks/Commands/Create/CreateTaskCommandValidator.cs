using FluentValidation;
using Taskio.Application.Common.Constants;

namespace Taskio.Application.Tasks.Commands.Create;

public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(ValidationConstants.TaskTitleLength);

        RuleFor(command => command.Description)
            .NotEmpty()
            .When(command => command.Description is not null)
            .MaximumLength(ValidationConstants.TaskDescriptionLength);

        RuleFor(command => command.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .When(command => command.DueDate is not null);

        RuleFor(command => command.UserId)
            .NotEmpty();

        RuleFor(command => command.Priority)
            .IsInEnum();

        RuleFor(command => command.Status)
            .IsInEnum();
    }
}
