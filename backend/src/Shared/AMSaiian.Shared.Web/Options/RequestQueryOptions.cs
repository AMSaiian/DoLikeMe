namespace AMSaiian.Shared.Web.Options;

public class RequestQueryOptions
{
    public const string SectionName = "RequestQuery";

    private const int PageNumber = 1;
    private const int PageSize = 10;
    private const bool IsDescending = true;

    public int DefaultPageNumber { get; set; } = PageNumber;

    public int DefaultPageSize { get; set; } = PageSize;

    public bool IsDescendingDefault { get; set; } = IsDescending;
}
