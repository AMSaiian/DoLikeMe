using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using AMSaiian.Shared.Domain.Interfaces;
using Task.io.Domain.Enums;

namespace Task.io.Domain.Entities;

public class Task : BaseEntity, IOrdering, IFiltered, IRanged
{
    public string Title { get; set; } = default!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public Status Status { get; set; } = Status.Pending;

    public Priority Priority { get; set; } = Priority.Medium;

    public Guid UserId { get; set; }

    public User User { get; set; } = default!;

    public static ReadOnlyDictionary<string, dynamic> OrderedBy { get; } = new(
        new Dictionary<string, dynamic>
        {
            { "title", (Expression<Func<Task, string>>)(ent => ent.Title) },
            { "priority", (Expression<Func<Task, Priority>>)(ent => ent.Priority) },
            { "status", (Expression<Func<Task, Status>>)(ent => ent.Status) },
            { "dueDate", (Expression<Func<Task, DateTime?>>)(ent => ent.DueDate) },
            { "createdDate", (Expression<Func<Task, DateTime>>)(ent => ent.CreatedAt) },
        });

    public static ReadOnlyDictionary<string, Func<HashSet<string>, dynamic>> FilteredBy { get; } = new(
        new Dictionary<string, Func<HashSet<string>, dynamic>>
        {
            {
                "priority", (Func<HashSet<string>, Expression<Func<Task, bool>>>)(filters =>
                    entity => filters.Select(Enum.Parse<Priority>).ToList().Contains(entity.Priority))
            },
            {
                "status", (Func<HashSet<string>, Expression<Func<Task, bool>>>)(filters =>
                    entity => filters.Select(Enum.Parse<Status>).Contains(entity.Status))
            },
            {
                "dueDate", (Func<HashSet<string>, Expression<Func<Task, bool>>>)(filters =>
                    entity => entity.DueDate.HasValue
                           && filters
                                  .Select(entry => DateTime
                                              .Parse(entry, CultureInfo.InvariantCulture))
                                  .Contains(entity.DueDate.Value))
            },
            {
                "createdDate", (Func<HashSet<string>, Expression<Func<Task, bool>>>)(filters =>
                    entity => filters
                        .Select(entry => DateTime
                                    .Parse(entry, CultureInfo.InvariantCulture))
                        .Contains(entity.CreatedAt))
            }
        });

    public static ReadOnlyDictionary<string, Func<string, string, dynamic>> RangedBy { get; } = new(
        new Dictionary<string, Func<string, string, dynamic>>
        {
            {
                "dueDate", (Func<string, string, Expression<Func<Task, bool>>>)((start, end) =>
                    entity => entity.DueDate.HasValue
                           && entity.DueDate >= DateTime.Parse(start, CultureInfo.InvariantCulture)
                           && entity.DueDate <= DateTime.Parse(end, CultureInfo.InvariantCulture))
            },
            {
                "createdDate", (Func<string, string, Expression<Func<Task, bool>>>)((start, end) =>
                    entity => entity.DueDate >= DateTime.Parse(start, CultureInfo.InvariantCulture)
                           && entity.DueDate <= DateTime.Parse(end, CultureInfo.InvariantCulture))
            }
        });
}
