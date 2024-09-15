using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskio.Domain.Entities;
using Taskio.Infrastructure.Persistence.Constants;
using Task = System.Threading.Tasks.Task;

namespace Taskio.Infrastructure.Persistence.Seeding.Initializers;

public class AppDbContextInitializer(ILogger<AppDbContextInitializer> logger,
                                     AppDbContext context,
                                     Faker<Domain.Entities.Task> taskFaker)
    : IAppDbContextInitializer
{
    public const int UserAmount = 5;
    public const int TasksPerUserAmount = 20;

    public List<Guid> UserIds { get; init; } =
    [
        new Guid("aecf89f1-0d5c-42bd-8e30-c84433e23b0b"),
        new Guid("11e8f79b-713e-449b-a4a3-bcc354ad48e6"),
        new Guid("3c0ecff1-3366-4974-aff5-96ee1472d32a"),
        new Guid("088a6d8a-88ee-455b-8587-cc575a572d6e"),
        new Guid("d5c64a11-11ae-4333-992f-e85666e6302f")
    ];

    public List<Guid> AuthIds { get; init; } =
    [
        new Guid("13617545-4390-4a80-a202-4144a56a082f"),
        new Guid("c1c1fd44-39b4-4c5a-9f66-c5e0dc38d579"),
        new Guid("5a64cb6e-9e2e-47ed-b21d-b1dddb11d9df"),
        new Guid("5daa400a-0dca-40c8-9833-6c10994c862a"),
        new Guid("c249e138-1488-43f2-af52-52a02019046d")
    ];

    public List<User> Users { get; init; } = [];

    public List<Domain.Entities.Task> Tasks { get; init; } = [];

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
            Tasks.Clear();
            Users.Clear();

            await SeedUsersAndTasks();
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ErrorMessagesConstants.SeedingError);
            throw;
        }
    }

    public async Task BulkClearStorageAsync()
    {
        await _context.Tasks
            .ExecuteDeleteAsync();

        await _context.Users
            .ExecuteDeleteAsync();
    }

    public Task ClearStorageAsync()
    {
        _context.Tasks.RemoveRange(_context.Tasks);
        _context.Users.RemoveRange(_context.Users);

        return _context.SaveChangesAsync();
    }

    private async Task SeedUsersAndTasks()
    {
        for (int i = 0; i < UserAmount; i++)
        {
            User newUser = new()
            {
                Id = UserIds[i],
                AuthId = AuthIds[i],
                Tasks = _taskFaker.Generate(TasksPerUserAmount)
            };

            Users.Add(newUser);
        }

        await _context.AddRangeAsync(Users);
    }

    private readonly ILogger<AppDbContextInitializer> _logger = logger;
    private readonly AppDbContext _context = context;
    private readonly Faker<Domain.Entities.Task> _taskFaker = taskFaker;

    private async Task<bool> CanBeSeeded()
    {
        return !(await _context.Users.AnyAsync()
              || await _context.Tasks.AnyAsync());
    }
}
