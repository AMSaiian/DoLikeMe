using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using AMSaiian.Shared.Domain.Interfaces;
using Taskio.Domain.Enums;

namespace Taskio.Domain.Entities;

public class Task : BaseEntity, IOrdering, IFiltered<Task>, IRanged<Task>
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

    public static ReadOnlyDictionary<string,
        Func<HashSet<string>,
            Expression<
                Func<Task, bool>>>> FilteredBy { get; } = new(
        new Dictionary<string, Func<HashSet<string>, Expression<Func<Task, bool>>>>
        {
            {
                "priority", filters =>
                    entity => filters.Select(filter => Enum.Parse<Priority>(filter, true)).ToList().Contains(entity.Priority)
            },
            {
                "status", filters =>
                    entity => filters.Select(filter => Enum.Parse<Status>(filter, true)).Contains(entity.Status)
            },
            {
                "dueDate", filters => entity =>
                    entity.DueDate.HasValue
                 && filters
                        .Select(entry => DateTime
                                    .Parse(entry, CultureInfo.InvariantCulture))
                        .Contains(entity.DueDate.Value)
            },
            {
                "createdDate", filters => entity => filters
                    .Select(entry => DateTime
                                .Parse(entry, CultureInfo.InvariantCulture))
                    .Contains(entity.CreatedAt)
            }
        });

    public static ReadOnlyDictionary<string,
        Func<string,
            string,
            Expression<Func<Task, bool>>>> RangedBy { get; } = new(
        new Dictionary<string, Func<string, string, Expression<Func<Task, bool>>>>
        {
            {
                "dueDate", (start, end) =>
                    entity => entity.DueDate.HasValue
                           && entity.DueDate >= DateTime.Parse(start, CultureInfo.InvariantCulture)
                           && entity.DueDate <= DateTime.Parse(end, CultureInfo.InvariantCulture)
            },
            {
                "createdDate", (start, end) =>
                    entity => entity.DueDate >= DateTime.Parse(start, CultureInfo.InvariantCulture)
                           && entity.DueDate <= DateTime.Parse(end, CultureInfo.InvariantCulture)
            }
        });
}
