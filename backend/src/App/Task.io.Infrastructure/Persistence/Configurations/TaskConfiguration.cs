using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task.io.Application.Common.Constants;
using Task.io.Domain.Enums;

namespace Task.io.Infrastructure.Persistence.Configurations;

public class TaskConfiguration : BaseEntityConfiguration<Domain.Entities.Task>
{
    public override void Configure(EntityTypeBuilder<Domain.Entities.Task> builder)
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
