using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IFiltered
{
    public static abstract ReadOnlyDictionary<string,
        Func<HashSet<string>, dynamic>> FilteredBy { get; }
}
