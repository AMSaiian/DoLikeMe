namespace Auth.Application.Common.Constants;

public static class LoggingTemplates
{
    public static readonly string UserRegistered = "New user with id {@NewUserId} created";
    public static readonly string UserProfileUpdated = "User with id {@UpdatedUserId} updated profile";
    public static readonly string UserPasswordUpdated = "User with id {@UpdatedUserId} updated password";
    public static readonly string UserSignedUp = "User with identifier {@Identifier} has been successfully signed up";
}
