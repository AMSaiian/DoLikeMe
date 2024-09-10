namespace Auth.Infrastructure.Persistence.Constraints;

public static class DataSchemeConstraints
{
    public static readonly string SchemeName = "auth";

    public static readonly string TimestampNowSql = "CURRENT_TIMESTAMP";
}
