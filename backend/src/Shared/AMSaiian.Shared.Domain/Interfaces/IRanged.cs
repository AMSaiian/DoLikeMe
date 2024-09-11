using System.Collections.ObjectModel;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IRanged
{
    public static abstract ReadOnlyDictionary<string, dynamic> RangedBy { get; }
}
