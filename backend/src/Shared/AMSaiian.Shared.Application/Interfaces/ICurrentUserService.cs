﻿namespace AMSaiian.Shared.Application.Interfaces;

public interface ICurrentUserService
{
    public Guid? UserId { get; }

    public Guid GetUserIdOrThrow();
}
