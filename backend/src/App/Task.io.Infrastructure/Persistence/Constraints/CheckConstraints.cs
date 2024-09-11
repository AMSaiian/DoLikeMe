namespace Task.io.Infrastructure.Persistence.Constraints;

public record CheckConstraint(string Name, string SqlCode);

public static class CheckConstraints
{
    public static readonly CheckConstraint UpdatedAtDateNotInTheFuture =
        new("CHK_{0}_update_at_datetime_not_in_future",
            "updated_at <= CURRENT_TIMESTAMP");

    public static readonly CheckConstraint CreatedAtDateNotInTheFuture =
        new("CHK_{0}_created_at_datetime_not_in_future",
            "created_at <= CURRENT_TIMESTAMP");

    public static readonly CheckConstraint CreatedAtDateNotBiggerThanUpdateAt =
        new("CHK_{0}_created_at_datetime_smaller_than_update_at",
            "created_at < updated_at");
}
