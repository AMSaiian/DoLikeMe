using Microsoft.EntityFrameworkCore;
using Task.io.Domain.Entities;

namespace Task.io.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Domain.Entities.Task> Tasks { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
