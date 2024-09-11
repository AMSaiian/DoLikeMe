namespace AMSaiian.Shared.Application.Models;

public record RangeContext
{
    public required string PropertyName { get; init; }
    public required string Start { get; init; }
    public required string End { get; init; }
}

