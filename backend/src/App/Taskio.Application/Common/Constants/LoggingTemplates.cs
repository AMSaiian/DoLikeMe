namespace Taskio.Application.Common.Constants;

public static class LoggingTemplates
{
    public static readonly string UserCreated = "New local user with id {@NewUserId} created";
    public static readonly string UserRequestedInfo = "User with auth id {@AuthUserId} requested user info";
    public static readonly string UserDeleted = "Local user with identifier {@Identifier} has been deleted";

    public static readonly string TaskCreated = "New task with id {@NewTaskId} created by local user with id {@UserId}";
    public static readonly string TaskDeleted = "Task with id {@DeletedTaskId} deleted by local user with id {@UserId}";
    public static readonly string TaskUpdated = "Task with id {@UpdatedTaskId} updated by local user with id {@UserId}";
}
