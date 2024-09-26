using Auth.Infrastructure;
using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Taskio.Infrastructure;
using Taskio.Infrastructure.Persistence;
using Taskio.IntegrationTests.Common.Constants;
using Testcontainers.PostgreSql;

namespace Taskio.IntegrationTests.Common;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
      IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage(DbContainerOptionsConstants.ImageName)
        .WithDatabase(DbContainerOptionsConstants.DatabaseName)
        .WithUsername(DbContainerOptionsConstants.Username)
        .WithPassword(DbContainerOptionsConstants.Password)
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Testing.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<IntegrationTestBase>()
                .AddEnvironmentVariables();
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));
            services.RemoveAll(typeof(DbContextOptions<AppIdentityDbContext>));

            services.AddAppDbContext(_dbContainer.GetConnectionString());
            services.AddAppIdentityDbContext(_dbContainer.GetConnectionString());
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.StopAsync();
        await base.DisposeAsync();
    }
}
