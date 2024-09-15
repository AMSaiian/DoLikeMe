namespace AMSaiian.Shared.Application.Models.Pagination;

public record PaginationContext
{
    public required PageContext PageContext { get; init; }
    public required OrderContext OrderContext { get; init; }
}
