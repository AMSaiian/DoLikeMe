namespace Auth.Infrastructure.Identity.Scopes;

public static class TaskioApplicationScopes
{
    public static readonly IList<string> DefaultScopes =
    [
        TaskResourceScopes.CreateTask,
        TaskResourceScopes.UpdateTaskDetails,
        TaskResourceScopes.UpdateTaskStatus,
        TaskResourceScopes.UpdateTaskPriorityAndDeadlines
    ];

    public static class TaskResourceScopes
    {
        public const string CreateTask = "task-create";
        public const string UpdateTaskDetails = "task-update";
        public const string UpdateTaskStatus = "task-status-update";
        public const string UpdateTaskPriorityAndDeadlines = "task-priority-deadlines-update";
    }
}
