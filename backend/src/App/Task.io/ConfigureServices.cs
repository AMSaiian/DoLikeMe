using System.Reflection;
using AMSaiian.Shared.Web.Filters;
using AMSaiian.Shared.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;
using Serilog.Core;
using Task.io.Common.Extensions;
using Task.io.Common.Options;

namespace Task.io;

public static class ConfigureServices
{
    public static IHostBuilder UseLogging(this IHostBuilder hostBuilder,
                                          IServiceCollection services,
                                          IConfigurationManager configuration)
    {
        services.AddSingleton<SensitiveLoggerPolicy>();

        hostBuilder.UseSerilog((context, provider, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
            loggerConfiguration.Destructure.With(provider.GetRequiredService<SensitiveLoggerPolicy>());
        });

        services.Configure<SensitiveLoggerOptions>(configuration
                                                       .GetSection(SensitiveLoggerOptions.SectionName));

        return hostBuilder;
    }

    public static IServiceCollection AddApiServices(this IServiceCollection services,
                                                    IConfigurationManager configuration)
    {
        services.AddHttpContextAccessor();

        services.AddProblemDetails();

        services
            .Configure<RouteOptions>(options =>
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
            .AddAutoMapper(mapperConfiguration =>
                               mapperConfiguration.AddMaps(Assembly.GetExecutingAssembly()))
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
