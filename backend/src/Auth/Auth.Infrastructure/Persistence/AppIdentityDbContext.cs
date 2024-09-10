using AMSaiian.Shared.Infrastructure.Extensions;
using Auth.Infrastructure.Persistence.Constraints;
using Auth.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence;

public class AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
    : IdentityDbContext<AuthUser, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(DataSchemeConstraints.SchemeName);

        builder.ApplyConfigurationsFromAssembly(typeof(AppIdentityDbContext).Assembly);

        builder.ToSnakeCaseConventions();
    }
}
