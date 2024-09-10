namespace Auth.Application.Common.Constants;

public static class ErrorMessagesConstants
{
    public static readonly string ForbiddenUpdateNotOwnedUser =
        "Can't update profile info of user which not owned by this account";

    public static readonly string NoChangesProvided = "No changes provided during update";
}
