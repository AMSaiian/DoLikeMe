using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using AMSaiian.Shared.Domain.Interfaces;
using Taskio.Domain.Constants;
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
            { TaskConstants.OrderedBy.Title, (Expression<Func<Task, string>>)(ent => ent.Title) },
            { TaskConstants.OrderedBy.Priority, (Expression<Func<Task, Priority>>)(ent => ent.Priority) },
            { TaskConstants.OrderedBy.Status, (Expression<Func<Task, Status>>)(ent => ent.Status) },
            { TaskConstants.OrderedBy.DueDate, (Expression<Func<Task, DateTime?>>)(ent => ent.DueDate) },
            { TaskConstants.OrderedBy.CreatedDate, (Expression<Func<Task, DateTime>>)(ent => ent.CreatedAt) },
            { TaskConstants.OrderedBy.UpdatedDate, (Expression<Func<Task, DateTime?>>)(ent => ent.UpdatedAt) },
        });

    public static ReadOnlyDictionary<string,
        Func<HashSet<string>,
            Expression<
                Func<Task, bool>>>> FilteredBy { get; } = new(
        new Dictionary<string, Func<HashSet<string>, Expression<Func<Task, bool>>>>
        {
            {
                TaskConstants.FilteredBy.Priority, filters =>
                    entity => filters
                        .Select(filter => Enum.Parse<Priority>(filter, true))
                        .Contains(entity.Priority)
            },
            {
                TaskConstants.FilteredBy.Status, filters =>
                    entity => filters
                        .Select(filter => Enum.Parse<Status>(filter, true))
                        .Contains(entity.Status)
            },
            {
                TaskConstants.FilteredBy.DueDate, filters => entity =>
                    entity.DueDate.HasValue
                 && filters
                        .Select(entry => DateTime
                                    .SpecifyKind(DateTime
                                                     .Parse(entry,
                                                            CultureInfo.InvariantCulture,
                                                            DateTimeStyles.AdjustToUniversal),
                                                 DateTimeKind.Utc))
                        .Contains(entity.DueDate.Value)
            },
            {
                TaskConstants.FilteredBy.CreatedDate, filters => entity => filters
                    .Select(entry => DateTime
                                .SpecifyKind(DateTime
                                                 .Parse(entry,
                                                        CultureInfo.InvariantCulture,
                                                        DateTimeStyles.AdjustToUniversal),
                                             DateTimeKind.Utc))
                    .Contains(entity.CreatedAt)
            },
            {
                TaskConstants.FilteredBy.UpdatedDate, filters => entity =>
                    entity.UpdatedAt.HasValue
                 && filters
                        .Select(entry => DateTime
                                    .SpecifyKind(DateTime
                                                     .Parse(entry,
                                                            CultureInfo.InvariantCulture,
                                                            DateTimeStyles.AdjustToUniversal),
                                                 DateTimeKind.Utc))
                        .Contains(entity.UpdatedAt.Value)
            }
        });

    public static ReadOnlyDictionary<string,
        Func<string,
            string,
            Expression<Func<Task, bool>>>> RangedBy { get; } = new(
        new Dictionary<string, Func<string, string, Expression<Func<Task, bool>>>>
        {
            {
                TaskConstants.RangedBy.DueDate, (start, end) =>
                    entity => entity.DueDate.HasValue
                           && entity.DueDate >= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(start,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
                           && entity.DueDate <= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(end,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
            },
            {
                TaskConstants.RangedBy.CreatedDate, (start, end) =>
                    entity => entity.CreatedAt >= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(start,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
                           && entity.CreatedAt <= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(end,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
            },
            {
                TaskConstants.RangedBy.UpdatedDate, (start, end) =>
                    entity => entity.UpdatedAt >= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(start,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
                           && entity.UpdatedAt <= DateTime
                                  .SpecifyKind(DateTime
                                                   .Parse(end,
                                                          CultureInfo.InvariantCulture,
                                                          DateTimeStyles.AdjustToUniversal),
                                               DateTimeKind.Utc)
            }
        });
}
