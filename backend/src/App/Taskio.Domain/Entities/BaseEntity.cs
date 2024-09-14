using AMSaiian.Shared.Domain.Interfaces;

namespace Taskio.Domain.Entities;

public abstract class BaseEntity : IAudited
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
