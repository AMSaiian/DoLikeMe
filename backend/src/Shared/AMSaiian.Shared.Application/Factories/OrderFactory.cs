using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Application.Templates;
using AMSaiian.Shared.Domain.Interfaces;
using FluentValidation.Results;

namespace AMSaiian.Shared.Application.Factories;

public class OrderFactory : IOrderFactory
{
    public IOrderedQueryable<TEntity> OrderDynamically<TEntity>(IQueryable<TEntity> source,
                                                                OrderContext context)
        where TEntity : IOrdering
    {
        TEntity.OrderedBy.TryGetValue(context.PropertyName, out dynamic? orderExpression);

        if (orderExpression is null)
        {
            throw new ValidationException([
                new ValidationFailure
                {
                    PropertyName = nameof(context.PropertyName),
                    ErrorMessage =
                        string.Format(ErrorTemplates.CantOrder, context.PropertyName)
                }
            ]);
        }

        IOrderedQueryable<TEntity> sortedQuery = context.IsDescending
            ? Queryable.OrderByDescending(source, orderExpression)
            : Queryable.OrderBy(source, orderExpression);

        return sortedQuery;
    }
}
