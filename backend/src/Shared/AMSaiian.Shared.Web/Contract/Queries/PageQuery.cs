using Microsoft.AspNetCore.Mvc;

namespace AMSaiian.Shared.Web.Contract.Queries;

public record PageQuery
{
    public const string Prefix = "pagination";

    [BindProperty(Name = Prefix + nameof(PageNumber))]
    public int? PageNumber { get; init; }

    [BindProperty(Name = Prefix + nameof(PageSize))]
    public int? PageSize { get; init; }
}
