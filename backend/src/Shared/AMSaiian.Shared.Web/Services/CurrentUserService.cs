using AMSaiian.Shared.Application.Interfaces;

namespace AMSaiian.Shared.Web.Services;

public class CurrentUserService : ICurrentUserService
{
    public Guid? UserId { get; }

    public Guid GetUserIdOrThrow()
    {
        throw new NotImplementedException();
    }
}
