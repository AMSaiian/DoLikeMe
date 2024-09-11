using System.Collections.ObjectModel;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IOrdering
{
    public static abstract ReadOnlyDictionary<string, dynamic> OrderedBy { get; }
}
