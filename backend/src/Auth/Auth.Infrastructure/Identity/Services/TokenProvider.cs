using System.Security.Claims;
using Auth.Infrastructure.Common.Interfaces;
using Auth.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Identity.Services;

public class TokenProvider(
    IOptionsSnapshot<TokenProviderOptions> options,
    JsonWebTokenHandler tokenHandler)
    : ITokenProvider
{
    private readonly IOptionsSnapshot<TokenProviderOptions> _options = options;
    private readonly JsonWebTokenHandler _tokenHandler = tokenHandler;

    public Task<string> CreateToken(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            Expires = DateTime.Now.AddMinutes(_options.Value.ExpiresInMinutes),
            Subject = principal.Identity as ClaimsIdentity,
            SigningCredentials = new SigningCredentials(_options.Value.GetParsedSecret(),
                                                        SecurityAlgorithms.HmacSha256)
        };

        string token = _tokenHandler.CreateToken(descriptor);

        return Task.FromResult(token);
    }
}
