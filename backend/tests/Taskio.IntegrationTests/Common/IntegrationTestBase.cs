using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Persistence.Seeding.Initializers;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Taskio.Application.Common.Interfaces;
using Taskio.Infrastructure.Persistence.Seeding.Initializers;

namespace Taskio.IntegrationTests.Common;

public abstract class IntegrationTestBase
    : IClassFixture<IntegrationTestWebAppFactory>,
      IDisposable
{
    protected WebApplicationFactory<Program> Factory { get; }

    protected IServiceScope Scope { get; }

    protected IAppDbContextInitializer DbInitializer { get; }

    protected IAppIdentityDbContextInitializer IdentityDbInitializer { get; }

    protected HttpClient HttpClient { get; }

    protected IMapper Mapper { get; }

    protected IAppDbContext DbContext { get; }

    protected AppIdentityDbContext IdentityDbContext { get; }

    protected IntegrationTestBase(IntegrationTestWebAppFactory factory)
    {
        Factory = factory;

        HttpClient = Factory.CreateClient();

        Scope = Factory.Services.CreateScope();

        Mapper = Scope.ServiceProvider
            .GetRequiredService<IMapper>();

        DbContext = Scope.ServiceProvider
            .GetRequiredService<IAppDbContext>();

        IdentityDbContext = Scope.ServiceProvider
            .GetRequiredService<AppIdentityDbContext>();

        DbInitializer = Scope.ServiceProvider
            .GetRequiredService<IAppDbContextInitializer>();
        IdentityDbInitializer = Scope.ServiceProvider
            .GetRequiredService<IAppIdentityDbContextInitializer>();
    }

    public async Task SetupDatabase()
    {
        await DbInitializer.ApplyDatabaseStructure();
        await DbInitializer.BulkClearStorageAsync();
        await DbInitializer.SeedAsync();

        await IdentityDbInitializer.ApplyDatabaseStructure();
        await IdentityDbInitializer.BulkClearStorageAsync();
        await IdentityDbInitializer.SeedAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Scope?.Dispose();
            DbContext?.Dispose();
            IdentityDbContext?.Dispose();
            HttpClient?.Dispose();
        }
    }
}
