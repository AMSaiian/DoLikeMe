using System.Security.Claims;
using System.Text.Json;
using Auth.Infrastructure.Common.Options;
using Auth.Infrastructure.Identity.Scopes;
using Auth.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Auth.Infrastructure.Identity.Services;

public class TaskioClaimsPrincipalFactory(
    IOptions<IdentityOptions> baseOptions,
    IOptionsSnapshot<TokenProviderOptions> options)
    : IUserClaimsPrincipalFactory<AuthUser>
{
    private readonly IOptions<IdentityOptions> _baseOptions = baseOptions;
    private readonly IOptionsSnapshot<TokenProviderOptions> _options = options;

    public async Task<ClaimsPrincipal> CreateAsync(AuthUser user)
    {
        ClaimsIdentity claimsIdentity = await GenerateClaimsAsync(user);

        return new ClaimsPrincipal(claimsIdentity);
    }

    private Task<ClaimsIdentity> GenerateClaimsAsync(AuthUser user)
    {
        List<Claim> claims =
        [
            new Claim(_baseOptions.Value.ClaimsIdentity.UserIdClaimType,
                      user.Id.ToString(),
                      ClaimValueTypes.String,
                      _options.Value.Issuer,
                      _options.Value.Issuer),

            new Claim(_baseOptions.Value.ClaimsIdentity.UserNameClaimType,
                      user.UserName!,
                      ClaimValueTypes.String,
                      _options.Value.Issuer,
                      _options.Value.Issuer),

            new Claim(_baseOptions.Value.ClaimsIdentity.EmailClaimType,
                      user.Email!,
                      ClaimValueTypes.Email,
                      _options.Value.Issuer,
                      _options.Value.Issuer),

            new Claim(_baseOptions.Value.ClaimsIdentity.SecurityStampClaimType,
                      user.SecurityStamp!,
                      ClaimValueTypes.String,
                      _options.Value.Issuer,
                      _options.Value.Issuer),

            new Claim(IdentityConstants.ScopesClaimType,
                      JsonSerializer
                          .Serialize(AuthApplicationScopes.DefaultScopes
                                         .Concat(TaskioApplicationScopes.DefaultScopes)),
                      JsonClaimValueTypes.JsonArray,
                      _options.Value.Issuer,
                      _options.Value.Issuer),
        ];

        var claimsIdentity = new ClaimsIdentity(
            claims,
            IdentityConstants.DefaultAuthenticationScheme,
            _baseOptions.Value.ClaimsIdentity.UserNameClaimType,
            _baseOptions.Value.ClaimsIdentity.RoleClaimType);

        return Task.FromResult(claimsIdentity);
    }
}
