using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Contract.Queries;

public record FilterQuery
{
    public const string Prefix = "filter";

    [BindProperty(Name = Prefix + nameof(PropertyName))]
    public string? PropertyName { get; init; }

    [BindProperty(Name = Prefix + nameof(Values))]
    public HashSet<string>? Values { get; init; }
}
