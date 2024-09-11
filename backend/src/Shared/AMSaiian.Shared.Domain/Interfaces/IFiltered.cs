using System.Collections.ObjectModel;

namespace AMSaiian.Shared.Domain.Interfaces;

public interface IFiltered
{
    public static abstract ReadOnlyDictionary<string, dynamic> FilteredBy { get; }
}
