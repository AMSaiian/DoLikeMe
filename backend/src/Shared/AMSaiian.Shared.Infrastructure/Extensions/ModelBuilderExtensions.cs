using Microsoft.EntityFrameworkCore;

namespace AMSaiian.Shared.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ToSnakeCaseConventions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model
            .GetEntityTypes()
            .Where(entity => !string.IsNullOrEmpty(modelBuilder
                                                       .Entity(entity.Name)
                                                       .Metadata
                                                       .GetTableName()))
            .ToList()
            .ForEach(entity =>
            {
                string? currentTableName = modelBuilder
                    .Entity(entity.Name)
                    .Metadata
                    .GetTableName();

                ArgumentNullException.ThrowIfNull(currentTableName);
                modelBuilder
                    .Entity(entity.Name)
                    .ToTable(currentTableName.ToSnakeCase());

                entity
                    .GetProperties()
                    .ToList()
                    .ForEach(property =>
                                 modelBuilder.Entity(entity.Name)
                                     .Property(property.Name)
                                     .HasColumnName(property.Name.ToSnakeCase()));
            });

        return modelBuilder;
    }
}
