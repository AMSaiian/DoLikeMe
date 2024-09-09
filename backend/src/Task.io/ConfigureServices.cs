using System.Reflection;
using AMSaiian.Shared.Web.Filters;
using AMSaiian.Shared.Web.Middlewares;

namespace Task.io;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddProblemDetails();

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
        });

        services.AddControllers(opts => opts.Filters.Add<ApiExceptionFilterAttribute>());

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddAutoMapper(configuration =>
                               configuration.AddMaps(Assembly.GetExecutingAssembly()))
            .AddCustomServices();

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        return services;
    }
}
