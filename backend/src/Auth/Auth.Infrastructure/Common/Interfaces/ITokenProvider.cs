using System.Security.Claims;

namespace Auth.Infrastructure.Common.Interfaces;

public interface ITokenProvider
{
    Task<string> CreateToken(ClaimsPrincipal principal, CancellationToken cancellationToken = default);
}
