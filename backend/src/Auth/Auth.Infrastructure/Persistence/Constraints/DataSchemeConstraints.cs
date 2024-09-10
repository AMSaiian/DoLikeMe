namespace Auth.Infrastructure.Persistence.Constraints;

public static class DataSchemeConstraints
{
    public static readonly string SchemeName = "auth";

    public static readonly string TimestampNowSql = "CURRENT_TIMESTAMP";

    public static readonly int UserEmailLength = 255;
    public static readonly int UserNameLength = 255;
}
