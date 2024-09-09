using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       string connectionString,
                                                       int seedingValue = 42)
    {
        services.AddAppDbContext(connectionString);

        return services;
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services,
                                                     string connectionString)
    {
        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        });
        services
            .AddDbContext<AppIdentityDbContext>(options =>
            {
                options
                    .UseNpgsql(connectionString);
            })
            .AddIdentityCore<IdentityUser<Guid>>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();


        return services;
    }
}
