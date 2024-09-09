using Microsoft.EntityFrameworkCore;

namespace AMSaiian.Shared.Infrastructure;

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
                    .ToTable(GetSnakeName(currentTableName));

                entity
                    .GetProperties()
                    .ToList()
                    .ForEach(property =>
                                 modelBuilder.Entity(entity.Name)
                                     .Property(property.Name)
                                     .HasColumnName(GetSnakeName(property.Name)));
            });

        return modelBuilder;
    }

    private static string GetSnakeName(string name)
    {
        return string.Concat(
            name.Select((x, i) => i > 0 && char.IsUpper(x)
                            ? $"_{x}"
                            : x.ToString())).ToLower();
    }
}
