using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskio.Application.Common.Constants;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : BaseEntityConfiguration<Task>
{
    public override void Configure(EntityTypeBuilder<Task> builder)
    {
        base.Configure(builder);

        builder
            .Property(task => task.Title)
            .HasMaxLength(ValidationConstants.TaskTitleLength)
            .IsRequired();

        builder
            .Property(task => task.Description)
            .HasMaxLength(ValidationConstants.TaskDescriptionLength)
            .IsRequired(false);

        builder
            .Property(task => task.Priority)
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(task => task.Status)
            .HasConversion<string>()
            .IsRequired();
    }
}
