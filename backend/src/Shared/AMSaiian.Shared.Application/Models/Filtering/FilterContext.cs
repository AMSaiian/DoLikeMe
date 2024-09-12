namespace AMSaiian.Shared.Application.Models.Filtering;

public record FilterContext
{
    public required string PropertyName { get; init; }
    public required HashSet<string> Filters { get; init; }
}
