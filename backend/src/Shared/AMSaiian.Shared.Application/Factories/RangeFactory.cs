using System.Linq.Expressions;
using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models;
using AMSaiian.Shared.Application.Templates;
using AMSaiian.Shared.Domain.Interfaces;
using FluentValidation.Results;

namespace AMSaiian.Shared.Application.Factories;

public class RangeFactory : IRangeFactory
{
    public IQueryable<TEntity> RangeDynamically<TEntity>(IQueryable<TEntity> source,
                                                         RangeContext context)
        where TEntity : IRanged<TEntity>
    {
        TEntity.RangedBy.TryGetValue(context.PropertyName,
                                     out Func<string, string, Expression<Func<TEntity, bool>>>? rangeFunction);

        if (rangeFunction is null)
        {
            throw new ValidationException([ new ValidationFailure
            {
                PropertyName = nameof(context.PropertyName),
                ErrorMessage = string.Format(ErrorTemplates.CantRanged, context.PropertyName)
            }]);
        }

        try
        {
            var rangeExpression = rangeFunction(context.Start, context.End);
            return source.Where(rangeExpression);
        }
        catch (Exception)
        {
            throw new ValidationException([ new ValidationFailure
            {
                PropertyName = string.Join(' ', nameof(context.Start), nameof(context.End)),
                ErrorMessage = ErrorTemplates.CantParseRange
            }]);
        }
    }
}
