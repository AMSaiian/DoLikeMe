using System.Reflection;
using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Task.io.Application.Common.Interfaces;
using Task.io.Domain.Entities;
using Task.io.Infrastructure.Persistence.Constraints;

namespace Task.io.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options),
                                                                    IAppDbContext
{
    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Domain.Entities.Task> Tasks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension(DataSchemeConstraints.KeyGenerationExtensionName);
        modelBuilder.HasDefaultSchema(DataSchemeConstraints.SchemeName);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ToSnakeCaseConventions();
    }
}
