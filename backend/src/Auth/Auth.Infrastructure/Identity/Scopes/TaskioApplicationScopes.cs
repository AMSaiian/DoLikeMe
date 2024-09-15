namespace Auth.Infrastructure.Identity.Scopes;

public static class TaskioApplicationScopes
{
    public static readonly IList<string> DefaultScopes =
    [
        TaskResourceScopes.CreateTask,
        TaskResourceScopes.DeleteTask,
        TaskResourceScopes.UpdateTask,
        TaskResourceScopes.GetOwnedTasksList,
        TaskResourceScopes.GetOwnedTaskDetails,
        UserResourceScopes.GetUserInfo
    ];

    public static class TaskResourceScopes
    {
        public const string GetOwnedTasksList = "task-get-own-list";
        public const string GetOwnedTaskDetails = "task-get-own-details";
        public const string CreateTask = "task-create";
        public const string DeleteTask = "task-delete";
        public const string UpdateTask = "task-update";
    }

    public static class UserResourceScopes
    {
        public const string GetUserInfo = "user-get-info";
    }
}
