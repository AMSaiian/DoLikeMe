using System.Reflection;
using AMSaiian.Shared.Application.Behaviours;
using AMSaiian.Shared.Application.Factories;
using AMSaiian.Shared.Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Taskio.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddHandlersAndBehaviour()
            .AddFluentValidators()
            .AddMapping()
            .AddCustomServices();

        return services;
    }

    private static IServiceCollection AddHandlersAndBehaviour(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        return services;
    }

    private static IServiceCollection AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddValidatorsFromAssembly(Assembly.Load("AMSaiian.Shared.Application"));

        return services;
    }

    private static IServiceCollection AddMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(configuration =>
                                   configuration.AddMaps(Assembly.GetExecutingAssembly()));

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IOrderFactory, OrderFactory>()
            .AddSingleton<IRangeFactory, RangeFactory>()
            .AddSingleton<IFilterFactory, FilterFactory>();

        return services;
    }
}
