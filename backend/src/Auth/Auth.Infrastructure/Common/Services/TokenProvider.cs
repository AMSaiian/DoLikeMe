using System.Security.Claims;
using System.Text;
using Auth.Infrastructure.Common.Interfaces;
using Auth.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Common.Services;

public class TokenProvider(IOptionsSnapshot<TokenProviderOptions> options,
                           JsonWebTokenHandler tokenHandler)
    : ITokenProvider
{
    private readonly IOptionsSnapshot<TokenProviderOptions> _options = options;
    private readonly JsonWebTokenHandler _tokenHandler = tokenHandler;

    public Task<string> CreateToken(IList<Claim> claims, CancellationToken cancellationToken = default)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = _options.Value.Issuer,
            Audience = _options.Value.Audience,
            Expires = DateTime.Now.AddMinutes(_options.Value.ExpiresInMinutes),
            Claims = claims.ToDictionary(claim => claim.Type, claim => claim.Value as object),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };

        string token = _tokenHandler.CreateToken(descriptor);

        return Task.FromResult(token);
    }
}
