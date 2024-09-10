using System.Security.Claims;

namespace Auth.Infrastructure.Common.Interfaces;

public interface ITokenProvider
{
    Task<string> CreateToken(IList<Claim> claims, CancellationToken cancellationToken = default);
}
