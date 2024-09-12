using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Domain.Interfaces;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AMSaiian.Shared.Infrastructure.Services;

public class PaginationService : IPaginationService
{

    public async Task<Paginated<TEntity>> PaginateAsync<TEntity>(
        IQueryable<TEntity> query,
        PaginationContext context,
        CancellationToken cancellationToken)
        where TEntity : IOrdering
    {
        IQueryable<TEntity> queryForChunking = TryOrderDynamically(query, context.OrderContext);

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

    public IOrderedQueryable<TEntity> TryOrderDynamically<TEntity>(
        IQueryable<TEntity> query,
        OrderContext context)
        where TEntity : IOrdering
    {
        TEntity.OrderedBy.TryGetValue(context.PropertyName, out dynamic? orderExpression);

        if (orderExpression is null)
        {
            throw new ValidationException([ new ValidationFailure
            {
                PropertyName = nameof(context.PropertyName),
                ErrorMessage = $"Can't order entity by {context.PropertyName}"
            }]);
        }

        IOrderedQueryable<TEntity> sortedQuery = context.IsDescending
            ? Queryable.OrderByDescending(query, orderExpression)
            : Queryable.OrderBy(query, orderExpression);

        return sortedQuery;
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
