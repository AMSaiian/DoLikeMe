using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Models.Token;
using Auth.Common.Contract.Requests.User;
using Auth.Infrastructure.Common.Options;
using Auth.Infrastructure.Identity;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Taskio.IntegrationTests.Common;
using Taskio.IntegrationTests.Common.Constants;

namespace Taskio.IntegrationTests.Tests.Users;

public class TokenEndpointTests
    : IntegrationTestBase,
      IClassFixture<JsonWebTokenHandler>
{
    public TokenEndpointTests(IntegrationTestWebAppFactory factory,
                              JsonWebTokenHandler jwtHandler)
        : base(factory)
    {
        _jwtHandler = jwtHandler;

        _tokenOptions = Scope.ServiceProvider
            .GetRequiredService<IOptions<TokenProviderOptions>>()
            .Value;
    }

    [Fact]
    public async Task LoginEndpointWithCorrectCredentialsMustReturnToken()
    {
        // Arrange
        await SetupDatabase();

        var requestUri = $"{ApiEndpointConstants.ApiBase}/{ApiEndpointConstants.AuthBase}/login";
        var request = new LoginRequest
        {
            Identifier = IdentityDbInitializer.Users[0].UserName,
            Password = "12345678Ab!"
        };

        // Act
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync(requestUri, request);

        // Assert
        TokenDto? tokenDto = await response.Content
            .ReadFromJsonAsync<TokenDto>();

        tokenDto
            .Should()
            .NotBeNull()
            .And.Subject.As<TokenDto>()
            .Value
            .Should()
            .NotBeEmpty();

        JsonWebToken decodedToken = _jwtHandler
            .ReadJsonWebToken(tokenDto!.Value);

        decodedToken
            .Should()
            .BeEquivalentTo(
                new
                {
                    Audiences = new List<string> { _tokenOptions.Audience },
                    Issuer = _tokenOptions.Issuer,
                    PayloadClaimNames = new[]
                    {
                        JwtRegisteredClaimNames.Aud,
                        JwtRegisteredClaimNames.Iss,
                        JwtRegisteredClaimNames.Exp,
                        ClaimTypes.NameIdentifier,
                        ClaimTypes.Name,
                        ClaimTypes.Email,
                        IdentityConstants.SecurityStampClaimType,
                        IdentityConstants.ScopesClaimType,
                        JwtRegisteredClaimNames.Iat,
                        JwtRegisteredClaimNames.Nbf,
                    },
                    Typ = "JWT"
                },
                options => options.ExcludingMissingMembers());
    }

    [Fact]
    public async Task LoginEndpointWithInCorrectCredentialsMustReturnUnauthorized()
    {
        // Arrange
        await SetupDatabase();

        var requestUri = $"{ApiEndpointConstants.ApiBase}/{ApiEndpointConstants.AuthBase}/login";
        var request = new LoginRequest
        {
            Identifier = IdentityDbInitializer.Users[0].UserName,
            Password = "123Invalid!",
        };

        // Act
        HttpResponseMessage response = await HttpClient
            .PostAsJsonAsync(requestUri, request);

        // Assert
        response
            .Should()
            .HaveStatusCode(HttpStatusCode.Unauthorized)
            .And
            .HaveClientError(ErrorMessagesConstants.InvalidPassword);
    }

    private readonly TokenProviderOptions _tokenOptions;

    private readonly JsonWebTokenHandler _jwtHandler;
}
