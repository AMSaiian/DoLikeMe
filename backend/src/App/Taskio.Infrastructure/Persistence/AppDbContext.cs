using System.Reflection;
using AMSaiian.Shared.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Taskio.Application.Common.Interfaces;
using Taskio.Domain.Entities;
using Taskio.Infrastructure.Persistence.Constraints;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options),
                                                                    IAppDbContext
{
    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Task> Tasks { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension(DataSchemeConstraints.KeyGenerationExtensionName);
        modelBuilder.HasDefaultSchema(DataSchemeConstraints.SchemeName);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ToSnakeCaseConventions();
    }
}
