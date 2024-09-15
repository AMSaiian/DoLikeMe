namespace Auth.Application.Common.Constants;

public static class ErrorMessagesConstants
{
    public static readonly string ForbiddenUpdateNotOwnedUser =
        "Can't update profile info of user which not owned by this account";

    public static readonly string ForbiddenDeleteNotOwnedUser = "Can't delete user which not owned by this account";

    public static readonly string NoChangesProvided = "No changes provided during update";

    public static readonly string IdentifierDataNotUnique = "Provided username or email is already used";

    public static readonly string UserNotFound = "User with provided id: {0} was not found";

    public static readonly string InvalidPassword = "Provided invalid password";
}
