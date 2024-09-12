using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IFiltered<TEntity>
{
    public static abstract ReadOnlyDictionary<string,
        Func<HashSet<string>, Expression<Func<TEntity, bool>>>> FilteredBy { get; }
}
