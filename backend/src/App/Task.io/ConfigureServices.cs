using System.Reflection;
using AMSaiian.Shared.Web.Filters;
using AMSaiian.Shared.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

        services.AddControllers(opts => opts.Filters.Add<ApiExceptionFilterAttribute>())
            .AddApplicationPart(Assembly.Load("Auth"));

        services.AddTunedAuth();

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddAutoMapper(configuration =>
                               configuration.AddMaps(Assembly.GetExecutingAssembly()))
            .AddCustomServices();

        return services;
    }

    private static IServiceCollection AddTunedAuth(this IServiceCollection services)
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
            });

        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        return services;
    }
}
