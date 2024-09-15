using Auth.Application.Common.Constants;
using Auth.Infrastructure.Persistence.Constraints;
using Auth.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Infrastructure.Persistence.Configurations;

public class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
{
    public void Configure(EntityTypeBuilder<AuthUser> builder)
    {
        builder.Property(user => user.CreatedAt)
            .HasDefaultValueSql(DataSchemeConstraints.TimestampNowSql);

        builder.Property(user => user.UserName)
            .HasMaxLength(ValidationConstants.UserNameLength)
            .IsRequired();
        builder.Property(user => user.NormalizedUserName)
            .HasMaxLength(ValidationConstants.UserNameLength)
            .IsRequired();

        builder.HasIndex(user => user.UserName)
            .IsUnique();
        builder.HasIndex(user => user.NormalizedUserName)
            .IsUnique();

        builder.Property(user => user.Email)
            .HasMaxLength(ValidationConstants.UserEmailLength)
            .IsRequired();
        builder.Property(user => user.NormalizedEmail)
            .HasMaxLength(ValidationConstants.UserEmailLength)
            .IsRequired();

        builder.HasIndex(user => user.Email)
            .IsUnique();
        builder.HasIndex(user => user.NormalizedEmail)
            .IsUnique();
    }
}
