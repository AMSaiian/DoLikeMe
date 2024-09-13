namespace AMSaiian.Shared.Application.Models.Pagination;

public record PaginationInfo
{
    public required int PageNumber { get; init; }
    public required int PageSize { get; init; }
    public required int TotalRecords { get; init; }
    public required int TotalPages { get; init; }
}
