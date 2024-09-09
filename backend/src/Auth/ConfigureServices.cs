using Auth.Application;
using Auth.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Auth;

public static class ConfigureServices
{
    public static IServiceCollection AddAuthProvider(this IServiceCollection services,
                                                     string connectionString)
    {
        services
            .AddApplicationServices()
            .AddInfrastructure(connectionString);

        return services;
    }
}
