using FluentValidation;
using Taskio.Application.Common.Constants;

namespace Taskio.Application.Tasks.Commands.Update;

public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Title)
            .NotEmpty()
            .When(command => command.Title is not null)
            .MaximumLength(ValidationConstants.TaskTitleLength);

        RuleFor(command => command.Description.Value)
            .NotEmpty()
            .When(command => command.Description is { IsDefined: true, Value: not null })
            .MaximumLength(ValidationConstants.TaskDescriptionLength);

        RuleFor(command => command.DueDate.Value)
            .NotEmpty()
            .When(command => command.DueDate is { IsDefined: true, Value: not null })
            .GreaterThanOrEqualTo(DateTime.UtcNow);

        RuleFor(command => command.Status)
            .IsInEnum()
            .When(command => command.Status is not null);

        RuleFor(command => command.Priority)
            .IsInEnum()
            .When(command => command.Priority is not null);
    }
}
