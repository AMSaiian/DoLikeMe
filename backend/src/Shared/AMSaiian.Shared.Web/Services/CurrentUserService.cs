using AMSaiian.Shared.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using UnauthorizedAccessException = AMSaiian.Shared.Application.Exceptions.UnauthorizedAccessException;

namespace AMSaiian.Shared.Web.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public Guid? UserId
    {
        get
        {
            string? authId = _httpContextAccessor.HttpContext?
                .User
                .Identity?
                .Name;

            return Guid.TryParse(authId, out Guid result) ? result : null;
        }
    }

    public Guid GetUserIdOrThrow()
    {
        string authId = _httpContextAccessor.HttpContext?
            .User
            .Identity?
            .Name ?? throw new UnauthorizedAccessException();

        return Guid.Parse(authId);
    }
}
