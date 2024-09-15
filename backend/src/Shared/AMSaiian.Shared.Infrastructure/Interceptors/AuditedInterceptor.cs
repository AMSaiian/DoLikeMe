using AMSaiian.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AMSaiian.Shared.Infrastructure.Interceptors;

public class AuditedInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        HandleUpdate(eventData);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        HandleUpdate(eventData);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleUpdate(DbContextEventData eventData)
    {
        IEnumerable<EntityEntry<IAudited>> entries = eventData.Context!.ChangeTracker
            .Entries<IAudited>()
            .Where(e => e.State is EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
