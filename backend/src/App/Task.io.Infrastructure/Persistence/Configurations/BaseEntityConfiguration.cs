using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task.io.Domain.Entities;
using Task.io.Infrastructure.Persistence.Constraints;

namespace Task.io.Infrastructure.Persistence.Configurations;

public abstract class BaseEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(ent => ent.Id);

        builder
            .Property(ent => ent.Id)
            .HasColumnType(DataSchemeConstraints.KeyType)
            .HasDefaultValueSql(DataSchemeConstraints.KeyTypeDefaultValue);

        builder.ToTable(table =>
        {
            table.HasCheckConstraint(string.Format(CheckConstraints.CreatedAtDateNotBiggerThanUpdateAt.Name,
                                                   typeof(TEntity).Name.ToSnakeCase()),
                                     CheckConstraints.CreatedAtDateNotBiggerThanUpdateAt.SqlCode);

            table.HasCheckConstraint(string.Format(CheckConstraints.CreatedAtDateNotInTheFuture.Name,
                                                   typeof(TEntity).Name.ToSnakeCase()),
                                     CheckConstraints.CreatedAtDateNotInTheFuture.SqlCode);

            table.HasCheckConstraint(string.Format(CheckConstraints.UpdatedAtDateNotInTheFuture.Name,
                                                   typeof(TEntity).Name.ToSnakeCase()),
                                     CheckConstraints.UpdatedAtDateNotInTheFuture.SqlCode);
        });

        builder
            .Property(ent => ent.CreatedAt)
            .HasDefaultValueSql(DataSchemeConstraints.TimestampNowSql)
            .IsRequired();

        builder
            .Property(ent => ent.UpdatedAt)
            .IsRequired(false);
    }
}
