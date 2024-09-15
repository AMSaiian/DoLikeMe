using AMSaiian.Shared.Infrastructure.Interceptors;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Infrastructure.Common.Interfaces;
using Auth.Infrastructure.Common.Options;
using Auth.Infrastructure.Identity.Services;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.Entities;
using Auth.Infrastructure.Persistence.Seeding.Fakers;
using Auth.Infrastructure.Persistence.Seeding.Initializers;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Auth.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                                       IConfigurationManager configuration,
                                                       string authConnectionStringName,
                                                       int seedingValue = 42)
    {
        string connectionString = configuration.GetConnectionString(authConnectionStringName)
                               ?? throw new ArgumentNullException(nameof(authConnectionStringName));

        services
            .AddAuthServices(configuration)
            .AddAppIdentityDbContext(connectionString)
            .AddAppDbContextInitializer(seedingValue);

        return services;
    }

    public static IServiceCollection AddAppIdentityDbContext(this IServiceCollection services,
                                                             string connectionString)
    {
        services
            .AddSingleton<SaveChangesInterceptor, AuditedInterceptor>()
            .AddDbContext<AppIdentityDbContext>((provider, options) =>
            {
                options
                    .UseNpgsql(connectionString)
                    .EnableSensitiveDataLogging(false)
                    .AddInterceptors(provider.GetRequiredService<SaveChangesInterceptor>());
            });

        return services;
    }

    private static IServiceCollection AddAppDbContextInitializer(this IServiceCollection services,
                                                                 int seedingValue)
    {
        Randomizer.Seed = new Random(seedingValue);

        services
            .AddScoped<IAppIdentityDbContextInitializer, AppIdentityDbContextInitializer>()
            .AddScoped<Faker<AuthUser>, AuthUserFaker>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services,
                                                      IConfigurationManager configuration)
    {
        services
            .Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = ValidationConstants.UserPasswordMinimumLength;
                options.User.RequireUniqueEmail = true;
            })
            .Configure<TokenProviderOptions>(configuration
                                                 .GetRequiredSection(TokenProviderOptions.SectionName))
            .AddScoped<ITokenProvider, TokenProvider>()
            .AddScoped<IAuthService, AuthService>()
            .AddScoped<JsonWebTokenHandler>()
            .AddIdentityCore<AuthUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddClaimsPrincipalFactory<TaskioClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

        return services;
    }
}
