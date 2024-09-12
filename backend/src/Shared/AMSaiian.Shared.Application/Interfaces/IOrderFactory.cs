using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Domain.Interfaces;

namespace AMSaiian.Shared.Application.Interfaces;

public interface IOrderFactory
{
    public IQueryable<TEntity> OrderDynamically<TEntity>(IQueryable<TEntity> source,
                                                         OrderContext context)
        where TEntity : IOrdering;
}
