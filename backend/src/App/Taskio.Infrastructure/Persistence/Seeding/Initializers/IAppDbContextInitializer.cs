using Taskio.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Taskio.Infrastructure.Persistence.Seeding.Initializers;

public interface IAppDbContextInitializer
{
    public List<User> Users { get; init; }

    public List<Domain.Entities.Task> Tasks { get; init; }

    public Task ApplyDatabaseStructure();

    public Task SeedAsync();

    public Task BulkClearStorageAsync();

    public Task ClearStorageAsync();
}
