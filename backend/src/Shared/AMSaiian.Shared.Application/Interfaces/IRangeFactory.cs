using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Domain.Interfaces;

namespace AMSaiian.Shared.Application.Interfaces;

public interface IRangeFactory
{
    public IQueryable<TEntity> RangeDynamically<TEntity>(IQueryable<TEntity> source,
                                                         RangeContext context)
        where TEntity : IRanged<TEntity>;
}
