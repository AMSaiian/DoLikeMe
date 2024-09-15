namespace AMSaiian.Shared.Application.Models.Pagination;

public record PageContext
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
}
