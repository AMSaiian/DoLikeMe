using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AMSaiian.Shared.Infrastructure.Services;

public class PaginationService(IOrderFactory factory) : IPaginationService
{
    private readonly IOrderFactory _factory = factory;

    public async Task<Paginated<TEntity>> PaginateAsync<TEntity>(
        IQueryable<TEntity> query,
        PaginationContext context,
        CancellationToken cancellationToken)
        where TEntity : IOrdering
    {
        IOrderedQueryable<TEntity> queryForChunking = _factory
            .OrderDynamically(query,
                              context.OrderContext);

        var chunkQuery = GetChunkQuery(queryForChunking, context.PageContext);
        var paginationInfo = await GetPaginationInfoAsync(query,
                                                          context.PageContext,
                                                          cancellationToken);

        List<TEntity> entities = await chunkQuery.ToListAsync(cancellationToken);

        Paginated<TEntity> paginated = new()
        {
            PaginationInfo = paginationInfo,
            Records = entities
        };

        return paginated;
    }

    public IQueryable<TEntity> GetChunkQuery<TEntity>(
        IQueryable<TEntity> query,
        PageContext context)
        where TEntity : IOrdering
    {
        int skipAmount = (context.PageNumber - 1) * context.PageSize;

        IQueryable<TEntity> chunkQuery = query
            .Skip(skipAmount)
            .Take(context.PageSize);

        return chunkQuery;
    }

    public async Task<PaginationInfo> GetPaginationInfoAsync<TEntity>(
        IQueryable<TEntity> initialQuery,
        PageContext context,
        CancellationToken cancellationToken)
        where TEntity : IOrdering
    {
        int totalRecords = await initialQuery.CountAsync(cancellationToken);
        int totalPages = (int)Math.Ceiling((decimal)totalRecords / context.PageSize);

        PaginationInfo info = new()
        {
            PageNumber = context.PageNumber,
            PageSize = context.PageSize,
            TotalPages = totalPages,
            TotalRecords = totalRecords,
        };

        return info;
    }
}
