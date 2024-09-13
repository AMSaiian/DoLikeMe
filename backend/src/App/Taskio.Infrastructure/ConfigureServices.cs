using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Infrastructure.Interceptors;
using AMSaiian.Shared.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskio.Application.Common.Interfaces;
using Taskio.Infrastructure.Persistence;

namespace Taskio.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationInfrastructure(this IServiceCollection services,
                                                                  IConfigurationManager configuration,
                                                                  string appConnectionStringName,
                                                                  int seedingValue = 42)
    {
        string connectionString = configuration.GetConnectionString(appConnectionStringName)
                               ?? throw new ArgumentNullException(nameof(appConnectionStringName));

        services
            .AddAppDbContext(connectionString);

        return services;
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services,
                                                      string connectionString)
    {
        services
            .AddSingleton<SaveChangesInterceptor, AuditedInterceptor>()
            .AddDbContext<AppDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging(false)
                    .AddInterceptors(provider.GetRequiredService<SaveChangesInterceptor>());
            })
            .AddSingleton<IPaginationService, PaginationService>()
            .AddScoped<IAppDbContext, AppDbContext>();

        return services;
    }
}
