using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Web.Filters;
using AMSaiian.Shared.Web.Middlewares;
using AMSaiian.Shared.Web.Options;
using AMSaiian.Shared.Web.Services;
using Auth.Infrastructure.Common.Options;
using Auth.Infrastructure.Identity;
using Auth.Infrastructure.Identity.Scopes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Taskio.Common.Extensions;
using Taskio.Common.Options;

namespace Taskio;

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
        services
            .Configure<RequestQueryOptions>(configuration
                                                .GetRequiredSection(RequestQueryOptions.SectionName));

        services.AddControllers(opts => opts.Filters.Add<ApiExceptionFilterAttribute>())
            .AddJsonOptions(opts =>
            {
                var enumConverter = new JsonStringEnumConverter();
                opts.JsonSerializerOptions.Converters.Add(enumConverter);
            })
            .AddApplicationPart(Assembly.Load("Auth"));

        services.AddTunedAuth(configuration);

        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddEndpointsApiExplorer()
            .AddTunedSwagger()
            .AddAutoMapper(mapperConfiguration =>
                               mapperConfiguration.AddMaps(Assembly.GetExecutingAssembly(),
                                                           Assembly.Load("AMSaiian.Shared.Web")))
            .AddCustomServices();

        return services;
    }

    private static IServiceCollection AddTunedAuth(this IServiceCollection services, IConfiguration configuration)
    {
        TokenProviderOptions? authOptions = configuration
            .GetSection(TokenProviderOptions.SectionName)
            .Get<TokenProviderOptions>();

        ArgumentNullException.ThrowIfNull(authOptions);

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    RequireAudience = true,
                    RequireSignedTokens = true,

                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = authOptions.GetParsedSecret(),
                    ClockSkew = TimeSpan.Zero,
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = ClaimTypes.Role,
                    AuthenticationType = JwtBearerDefaults.AuthenticationScheme,
                };

                options.ClaimsIssuer = authOptions.Issuer;
                options.Audience = authOptions.Audience;
                options.MapInboundClaims = true;
            });

        services.AddAuthorization(options =>
        {
            var allApplicationScopes =
                AuthApplicationScopes.DefaultScopes
                    .Concat(TaskioApplicationScopes.DefaultScopes);

            foreach (string scope in allApplicationScopes)
            {
                options
                    .AddPolicy(scope,
                               policy => policy
                                   .RequireClaim(IdentityConstants.ScopesClaimType, scope));
            }
        });

        return services;
    }

    private static IServiceCollection AddTunedSwagger(this IServiceCollection services)
    {
        return services
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        []
                    }
                });
            });
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
