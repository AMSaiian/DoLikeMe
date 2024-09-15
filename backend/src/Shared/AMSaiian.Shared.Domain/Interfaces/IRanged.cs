using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IRanged<TEntity>
{
    public static abstract ReadOnlyDictionary<string,
        Func<string,
            string,
            Expression<Func<TEntity, bool>>>> RangedBy { get; }
}
