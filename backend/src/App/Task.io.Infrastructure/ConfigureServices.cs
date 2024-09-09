using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Task.io.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string configuration)
    {
        return services;
    }
}
