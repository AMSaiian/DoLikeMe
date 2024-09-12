using AMSaiian.Shared.Web.Controllers;
using Auth.Application.Users.Commands.Register;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskio.Common.Contract.Requests.User;

namespace Taskio.Controllers;

public class UsersController(ISender mediator, IMapper mapper)
    : ApiControllerBase(mediator, mapper)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request,
                                              CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<RegisterUserCommand>(request);

        Guid result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
