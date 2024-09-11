using AMSaiian.Shared.Web.Controllers;
using Auth.Application.Users.Commands.SignUp;
using Auth.Common.Contract.Requests.User;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers;

public class AuthController(ISender mediator, IMapper mapper)
    : ApiControllerBase(mediator, mapper)
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
                                           CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<SignUpCommand>(request);

        string result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
