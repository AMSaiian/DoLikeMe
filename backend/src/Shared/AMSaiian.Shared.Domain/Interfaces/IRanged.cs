using System.Collections.ObjectModel;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IRanged
{
    public static abstract ReadOnlyDictionary<string, Func<string, string, dynamic>> RangedBy { get; }
}
