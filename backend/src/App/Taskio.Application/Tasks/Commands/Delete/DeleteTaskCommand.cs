using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Taskio.Application.Tasks.Commands.Delete;

public record DeleteTaskCommand : IRequest
{
    public required Guid Id { get; init; }
}

public sealed class DeleteTaskHandler(
    IAppDbContext dbContext,
    ICurrentUserService currentUserService,
    ILogger<DeleteTaskHandler> logger)
    : IRequestHandler<DeleteTaskCommand>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ILogger<DeleteTaskHandler> _logger = logger;

    public async Task Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        Guid authUserId = _currentUserService
            .GetUserIdOrThrow();

        Domain.Entities.Task deletingTask = await _dbContext.Tasks
                                                .Include(t => t.User)
                                                .FirstOrDefaultAsync(t => t.Id == request.Id,
                                                                     cancellationToken)
                                         ?? throw new NotFoundException(
                                                string.Format(ErrorMessagesConstants.TaskNotFound,
                                                              request.Id));

        if (deletingTask.User.AuthId != authUserId)
        {
            throw new ForbiddenAccessException(
                ErrorMessagesConstants.ForbiddenDeleteNotOwnedTask);
        }

        Guid ownerId = deletingTask.User.Id;

        _dbContext.Tasks.Remove(deletingTask);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LoggingTemplates.TaskDeleted,
                               request.Id,
                               ownerId);
    }
}
