using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Contract.Queries;

public record RangeQuery
{
    public const string Prefix = "range";

    [BindProperty(Name = Prefix + nameof(PropertyName))]
    public string? PropertyName { get; init; }

    [BindProperty(Name = Prefix + nameof(Start))]
    public string? Start { get; init; }

    [BindProperty(Name = Prefix + nameof(End))]
    public string? End { get; init; }
}
