using Auth.Infrastructure.Persistence.Constants;
using Auth.Infrastructure.Persistence.Entities;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task = System.Threading.Tasks.Task;

namespace Auth.Infrastructure.Persistence.Seeding.Initializers;

public class AppIdentityDbContextInitializer(ILogger<AppIdentityDbContextInitializer> logger,
                                     AppIdentityDbContext context,
                                     Faker<AuthUser> taskFaker)
    : IAppIdentityDbContextInitializer
{
    public const int UserAmount = 5;

    public List<Guid> AuthIds { get; init; } =
    [
        new Guid("13617545-4390-4a80-a202-4144a56a082f"),
        new Guid("c1c1fd44-39b4-4c5a-9f66-c5e0dc38d579"),
        new Guid("5a64cb6e-9e2e-47ed-b21d-b1dddb11d9df"),
        new Guid("5daa400a-0dca-40c8-9833-6c10994c862a"),
        new Guid("c249e138-1488-43f2-af52-52a02019046d")
    ];

    public List<AuthUser> Users { get; init; } = [];

    public async Task ApplyDatabaseStructure()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessagesConstants.MigrationError);
            throw;
        }
    }

    public async Task SeedAsync()
    {
        if (!await CanBeSeeded())
        {
            return;
        }

        try
        {
            Users.Clear();

            await SeedUsers();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessagesConstants.SeedingError);
            throw;
        }
    }

    public Task BulkClearStorageAsync()
    {
        return _context.Users
            .ExecuteDeleteAsync();
    }

    public Task ClearStorageAsync()
    {
        _context.Users.RemoveRange(_context.Users);

        return _context.SaveChangesAsync();
    }

    private readonly ILogger<AppIdentityDbContextInitializer> _logger = logger;
    private readonly AppIdentityDbContext _context = context;
    private readonly Faker<AuthUser> _taskFaker = taskFaker;

    private async Task SeedUsers()
    {
        Users.AddRange(_taskFaker.Generate(UserAmount));

        for (int i = 0; i < UserAmount; i++)
        {
            Users[i].Id = AuthIds[i];
        }

        await _context.AddRangeAsync(Users);
    }

    private async Task<bool> CanBeSeeded()
    {
        return !await _context.Users.AnyAsync();
    }
}
