namespace Auth.Infrastructure.Identity.Scopes;

public static class AuthApplicationScopes
{
    public static readonly IList<string> DefaultScopes =
    [
        UserResourceScopes.UpdateUserProfile,
        UserResourceScopes.UpdateUserPassword,
        UserResourceScopes.DeleteUserProfile
    ];

    public static class UserResourceScopes
    {
        public const string UpdateUserProfile = "user-profile-update";
        public const string UpdateUserPassword = "user-password-update";
        public const string DeleteUserProfile = "user-profile-delete";
    }
}
