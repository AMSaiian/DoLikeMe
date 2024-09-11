using AMSaiian.Shared.Domain.Interfaces;

namespace Task.io.Domain.Entities;

public abstract class BaseEntity : IAudited
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
