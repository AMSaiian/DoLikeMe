using Bogus;
using Bogus.Extensions;
using Taskio.Application.Common.Constants;
using Taskio.Domain.Enums;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Infrastructure.Persistence.Seeding.Fakers;

public sealed class TaskFaker : Faker<Task>
{
    public TaskFaker()
    {
        RuleFor(t => t.Title,
                f => f.Lorem
                    .Sentence(3, 5)
                    .ClampLength(max: ValidationConstants.TaskDescriptionLength));

        RuleFor(t => t.Description, f => f.Lorem
                    .Paragraph(5)
                    .ClampLength(max: ValidationConstants.TaskDescriptionLength)
                    .OrNull(f, 0.4F));

        RuleFor(t => t.DueDate,
                f => f.Date
                    .Soon(15, DateTime.UtcNow.AddDays(1))
                    .OrNull(f, 0.3F));

        RuleFor(t => t.CreatedAt,
                f => f.Date
                    .Recent(10, DateTime.UtcNow.AddDays(-10)));

        RuleFor(t => t.UpdatedAt,
                (f, t) => f.Date
                    .Soon(9, t.CreatedAt)
                    .OrNull(f, 0.2F));

        RuleFor(t => t.Status,
                f => f.PickRandom<Status>());

        RuleFor(t => t.Priority,
                f => f.PickRandom<Priority>());
    }
}
