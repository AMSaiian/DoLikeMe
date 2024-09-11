using System.Reflection;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Web.Services;
using Auth.Application;
using Auth.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth;

public static class ConfigureServices
{
    public static IServiceCollection AddAuthProvider(this IServiceCollection services,
                                                     IConfigurationManager configuration,
                                                     string authConnectionStringName)
    {
        services
            .AddWebServices()
            .AddApplicationServices()
            .AddInfrastructure(configuration, authConnectionStringName);

        return services;
    }

    private static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services
            .AddAutoMapper(configuration =>
                               configuration.AddMaps(Assembly.GetExecutingAssembly()))
            .AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
