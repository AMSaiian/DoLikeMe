using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Contract.Queries;

public record OrderQuery
{
    public const string Prefix = "order";

    [BindProperty(Name = Prefix + nameof(PropertyName))]
    public string? PropertyName { get; init; }

    [BindProperty(Name = Prefix + nameof(IsDescending))]
    public bool? IsDescending { get; init; }
}
