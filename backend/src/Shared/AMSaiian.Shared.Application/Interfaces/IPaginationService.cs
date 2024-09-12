using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Domain.Interfaces;

namespace AMSaiian.Shared.Application.Interfaces;

public interface IPaginationService
{
    public Task<Paginated<TEntity>> PaginateAsync<TEntity>(
        IQueryable<TEntity> query,
        PaginationContext context,
        CancellationToken cancellationToken)
        where TEntity : IOrdering;

    public IQueryable<TEntity> GetChunkQuery<TEntity>(
        IQueryable<TEntity> query,
        PageContext context)
        where TEntity : IOrdering;

    public Task<PaginationInfo> GetPaginationInfoAsync<TEntity>(
        IQueryable<TEntity> initialQuery,
        PageContext context,
        CancellationToken cancellationToken)
        where TEntity : IOrdering;
}
