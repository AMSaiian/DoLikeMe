using Microsoft.EntityFrameworkCore;
using Taskio.Domain.Entities;

namespace Taskio.Application.Common.Interfaces;

public interface IAppDbContext : IDisposable
{
    public DbSet<User> Users { get; set; }

    public DbSet<Taskio.Domain.Entities.Task> Tasks { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
