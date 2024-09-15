using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Taskio.Domain.Entities;
using Taskio.Infrastructure.Persistence.Constraints;

namespace Taskio.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BaseEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);

        builder
            .Property(user => user.AuthId)
            .HasColumnType(DataSchemeConstraints.KeyType)
            .IsRequired();

        builder
            .HasMany(user => user.Tasks)
            .WithOne(task => task.User)
            .HasForeignKey(task => task.UserId)
            .IsRequired();
    }
}
