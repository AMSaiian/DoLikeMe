using AMSaiian.Shared.Web.Controllers;
using Auth.Application.Common.Models.Token;
using Auth.Application.Users.Commands.ChangePassword;
using Auth.Application.Users.Commands.SignUp;
using Auth.Application.Users.Commands.UpdateProfileInfo;
using Auth.Common.Contract.Requests.User;
using Auth.Infrastructure.Identity.Scopes;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

public class AuthController(ISender mediator, IMapper mapper)
    : ApiControllerBase(mediator, mapper)
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
                                           CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<SignUpCommand>(request);

        TokenDto result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [Authorize(Policy = AuthApplicationScopes.UserResourceScopes.UpdateUserProfile)]
    [HttpPut("accounts/{userId:guid}/identifier")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateIdentifier([FromRoute] Guid userId,
                                                      [FromBody] UpdateIdentifierRequest request,
                                                      CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<UpdateProfileInfoCommand>(request) with { Id = userId };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [Authorize(Policy = AuthApplicationScopes.UserResourceScopes.UpdateUserPassword)]
    [HttpPut("accounts/{userId:guid}/password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid userId,
                                                    [FromBody] ChangePasswordRequest request,
                                                    CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<ChangePasswordCommand>(request) with { Id = userId };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}
