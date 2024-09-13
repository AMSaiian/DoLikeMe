using Auth.Infrastructure.Persistence.Entities;

namespace Auth.Infrastructure.Persistence.Seeding.Initializers;

public interface IAppIdentityDbContextInitializer
{
    public List<AuthUser> Users { get; init; }

    public Task ApplyDatabaseStructure();

    public Task SeedAsync();

    public Task BulkClearStorageAsync();

    public Task ClearStorageAsync();
}
