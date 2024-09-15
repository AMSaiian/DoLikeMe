namespace AMSaiian.Shared.Application.Models.Pagination;

public record Paginated<T>
{
    public required PaginationInfo PaginationInfo { get; init; }
    public required List<T> Records { get; init; }
}
