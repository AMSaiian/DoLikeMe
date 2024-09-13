using AMSaiian.Shared.Web.Controllers;
using Auth.Infrastructure.Identity.Scopes;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskio.Application.Common.Models.Task;
using Taskio.Application.Tasks.Commands.Create;
using Taskio.Application.Tasks.Commands.Delete;
using Taskio.Application.Tasks.Commands.Update;
using Taskio.Application.Tasks.Queries.GetDetailed;
using Taskio.Common.Contract.Requests.Task;

namespace Taskio.Controllers;

public class TasksController(
    ISender mediator,
    IMapper mapper)
    : ApiControllerBase(mediator, mapper)
{
    [Authorize(Policy = TaskioApplicationScopes.TaskResourceScopes.GetOwnedTaskDetails)]
    [HttpGet("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskFullDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserTasks([FromRoute] Guid taskId,
                                                  CancellationToken cancellationToken = default)
    {
        var command = new GetDetailedTaskQuery
        {
            Id = taskId
        };

        TaskFullDto result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [Authorize(Policy = TaskioApplicationScopes.TaskResourceScopes.UpdateTask)]
    [HttpPut("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTask([FromRoute] Guid taskId,
                                                [FromBody] UpdateTaskRequest request,
                                                CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<UpdateTaskCommand>(request) with { Id = taskId };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [Authorize(Policy = TaskioApplicationScopes.TaskResourceScopes.CreateTask)]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request,
                                                CancellationToken cancellationToken = default)
    {
        var command = _mapper.Map<CreateTaskCommand>(request);

        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [Authorize(Policy = TaskioApplicationScopes.TaskResourceScopes.DeleteTask)]
    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId,
                                                CancellationToken cancellationToken = default)
    {
        var command = new DeleteTaskCommand { Id = taskId };

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}
