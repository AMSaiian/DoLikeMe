using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Application.Models.Pagination;
using AMSaiian.Shared.Web.Contract.Queries;
using AMSaiian.Shared.Web.Controllers;
using AMSaiian.Shared.Web.Extensions;
using AMSaiian.Shared.Web.Options;
using Auth.Application.Users.Commands.Register;
using Auth.Infrastructure.Identity.Scopes;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Taskio.Application.Common.Models.Task;
using Taskio.Application.Tasks.Queries.GetPaginated;
using Taskio.Application.Users.Commands.Create;
using Taskio.Application.Users.Commands.Delete;
using Taskio.Common.Contract.Requests.User;
using Taskio.Domain.Constants;

namespace Taskio.Controllers;

public class UsersController(
    ISender mediator,
    IMapper mapper,
    IOptions<RequestQueryOptions> queryOptions)
    : ApiControllerBase(mediator, mapper)
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request,
                                                CancellationToken cancellationToken = default)
    {
        var registerIdentityCommand = _mapper.Map<RegisterUserCommand>(request);

        Guid authId = await _mediator.Send(registerIdentityCommand, cancellationToken);

        cancellationToken = CancellationToken.None;

        var createLocalUserCommand = new CreateUserCommand
        {
            AuthId = authId,
        };

        Guid localUserId = await _mediator.Send(createLocalUserCommand, cancellationToken);

        return Ok(localUserId);
    }

    [Authorize(Policy = AuthApplicationScopes.UserResourceScopes.DeleteUserProfile)]
    [HttpPost("{userId:guid}/delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid userId,
                                                [FromBody] DeleteUserRequest request,
                                                CancellationToken cancellationToken = default)
    {
        var deleteLocalUserCommand = new DeleteUserCommand { Id = userId };

        Guid deletingAuthId = await _mediator.Send(deleteLocalUserCommand, cancellationToken);

        cancellationToken = CancellationToken.None;

        var deleteAuthUserCommand =
            _mapper.Map<Auth.Application.Users.Commands.Delete.DeleteUserCommand>(request)
                with { Id = deletingAuthId };

        await _mediator.Send(deleteAuthUserCommand, cancellationToken);

        return Ok();
    }

    [Authorize(Policy = TaskioApplicationScopes.TaskResourceScopes.GetOwnedTasksList)]
    [HttpGet("{userId:guid}/tasks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Paginated<TaskShortDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserTasks([FromRoute] Guid userId,
                                                  [FromQuery] PageQuery page,
                                                  [FromQuery] OrderQuery order,
                                                  [FromQuery] FilterQuery filter,
                                                  [FromQuery] RangeQuery range,
                                                  CancellationToken cancellationToken = default)
    {
        const string defaultOrderPropertyName = TaskConstants.OrderedBy.CreatedDate;

        PaginationContext paginationContext = new()
        {
            OrderContext = this.ProcessOrderQuery(order, defaultOrderPropertyName, _mapper),
            PageContext = this.ProcessPageQuery(page, _mapper, queryOptions.Value)
        };
        FilterContext? filterContext = this.ProcessFilterQuery(filter, _mapper);
        RangeContext? rangeContext = this.ProcessRangeQuery(range, _mapper);

        var command = new GetPaginatedTasksQuery
        {
            PaginationContext = paginationContext,
            FilterContext = filterContext,
            RangeContext = rangeContext,
            UserId = userId
        };

        Paginated<TaskShortDto> result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
