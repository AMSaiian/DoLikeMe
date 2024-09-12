using System.Linq.Expressions;
using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models;
using AMSaiian.Shared.Application.Templates;
using AMSaiian.Shared.Domain.Interfaces;
using FluentValidation.Results;

namespace AMSaiian.Shared.Application.Factories;

public class FilterFactory : IFilterFactory
{
    public IQueryable<TEntity> FilterDynamically<TEntity>(IQueryable<TEntity> source,
                                                          FilterContext context)
        where TEntity : IFiltered<TEntity>
    {
        TEntity.FilteredBy.TryGetValue(context.PropertyName,
                                       out Func<HashSet<string>,
                                           Expression<Func<TEntity, bool>>>? filterFunction);

        if (filterFunction is null)
        {
            throw new ValidationException([ new ValidationFailure
            {
                PropertyName = nameof(context.PropertyName),
                ErrorMessage = string.Format(ErrorTemplates.CantFiltered, context.PropertyName)
            }]);
        }

        try
        {
            Expression<Func<TEntity, bool>> filterExpression = filterFunction(context.Filters);
            return source.Where(filterExpression);
        }
        catch (Exception)
        {
            throw new ValidationException([ new ValidationFailure
            {
                PropertyName = nameof(context.Filters),
                ErrorMessage = ErrorTemplates.CantParseFilters
            }]);
        }
    }
}
