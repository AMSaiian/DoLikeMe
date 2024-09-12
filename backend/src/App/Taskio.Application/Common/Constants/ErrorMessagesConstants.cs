﻿namespace Taskio.Application.Common.Constants;

public static class ErrorMessagesConstants
{
    public static readonly string ForbiddenUpdateNotOwnedUser =
        "Can't update profile info of user which not owned by this account";

    public static readonly string ForbiddenDeleteNotOwnedUser = "Can't delete user which not owned by this account";

    public static readonly string NoChangesProvided = "No changes provided during update";

    public static readonly string IdentifierDataNotUnique = "User with provided auth id is already exists";

    public static readonly string UserNotFound = "User with provided id: {0} was not found";

    public static readonly string TaskNotFound = "Task with provided id: {0} not found";

    public static readonly string ForbiddenCreateTasksForAnotherUser = "Can't create tasks for another user";

    public static readonly string ForbiddenDeleteNotOwnedTask = "Can't delete task which not owned by this account";

    public static readonly string ForbiddenUpdateNotOwnedTask = "Can't update task which not owned by this account";
}