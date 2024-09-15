namespace Auth.Application.Common.Constants;

public static class ValidationConstants
{
    public static readonly int UserEmailLength = 255;
    public static readonly int UserNameLength = 255;

    public static readonly int UserPasswordMinimumLength = 8;
    public static readonly int UserPasswordMaximumLength = 20;

    public static readonly string UserPasswordRegex =
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$";

    public static readonly string UserNameRegex = @"^[a-zA-Z0-9\-._@+]+$";
}
