using AMSaiian.Shared.Application.Models;
using AMSaiian.Shared.Domain.Interfaces;

namespace AMSaiian.Shared.Application.Interfaces;

public interface IFilterFactory
{
    public IQueryable<TEntity> FilterDynamically<TEntity>(IQueryable<TEntity> source,
                                                          FilterContext context)
        where TEntity : IFiltered;
}
