using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace Taskio.Application.Users.Commands.Delete;

public record DeleteUserCommand : IRequest
{
    public required Guid Id { get; init; }
}

public class DeleteUserHandler(IAppDbContext dbContext,
                                      ICurrentUserService currentUser,
                                      ILogger<DeleteUserHandler> logger)
    : IRequestHandler<DeleteUserCommand>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ILogger<DeleteUserHandler> _logger = logger;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        Guid currentUserId = _currentUser.GetUserIdOrThrow();

        User deletingUser = await _dbContext.Users.FindAsync(request.Id, cancellationToken)
                          ?? throw new NotFoundException(
                                string.Format(ErrorMessagesConstants.UserNotFound,
                                              request.Id));

        if (deletingUser.AuthId != currentUserId)
        {
            throw new ForbiddenAccessException(ErrorMessagesConstants.ForbiddenDeleteNotOwnedUser);
        }

        _dbContext.Users.Remove(deletingUser);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LoggingTemplates.UserDeleted, request.Id);
    }
}
