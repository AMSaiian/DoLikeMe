using System.Security.Claims;

namespace Auth.Infrastructure.Common.Interfaces;

public interface ITokenProvider
{
    public string CreateToken(IList<Claim> claims);
}
