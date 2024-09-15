namespace AMSaiian.Shared.Domain.Interfaces;

public interface IAudited
{
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
