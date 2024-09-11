using Microsoft.Extensions.DependencyInjection;

namespace Task.io.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services;
    }
}
