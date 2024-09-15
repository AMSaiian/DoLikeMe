using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Infrastructure.Interceptors;
using AMSaiian.Shared.Infrastructure.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Taskio.Application.Common.Interfaces;
using Taskio.Infrastructure.Persistence;
using Taskio.Infrastructure.Persistence.Seeding.Fakers;
using Taskio.Infrastructure.Persistence.Seeding.Initializers;
using Task = Taskio.Domain.Entities.Task;

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
            .AddAppDbContext(connectionString)
            .AddAppDbContextInitializer(seedingValue);

        return services;
    }

    public static IServiceCollection AddAppDbContext(this IServiceCollection services,
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

    private static IServiceCollection AddAppDbContextInitializer(this IServiceCollection services,
                                                                 int seedingValue)
    {
        Randomizer.Seed = new Random(seedingValue);

        services
            .AddScoped<IAppDbContextInitializer, AppDbContextInitializer>()
            .AddScoped<Faker<Task>, TaskFaker>();

        return services;
    }
}
