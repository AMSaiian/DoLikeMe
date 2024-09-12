using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Wrappers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Domain.Enums;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Taskio.Application.Tasks.Commands.Update;

public record UpdateTaskCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? Title { get; init; }
    public Undefinable<string?> Description { get; init; }
    public Undefinable<DateTime?> DueDate { get; init; }
    public Status? Status { get; init; }
    public Priority? Priority { get; init; }
}

public class UpdateTaskHandler(IAppDbContext dbContext,
                               ICurrentUserService currentUserService,
                               IMapper mapper,
                               ILogger<UpdateTaskHandler> logger)
    : IRequestHandler<UpdateTaskCommand>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateTaskHandler> _logger = logger;

    public async Task Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        Guid authUserId = _currentUserService
            .GetUserIdOrThrow();

        Domain.Entities.Task updatingTask = await _dbContext.Tasks
                                                .Include(t => t.User)
                                                .FirstOrDefaultAsync(t => t.Id == request.Id,
                                                                     cancellationToken)
                                         ?? throw new NotFoundException(
                                                string.Format(ErrorMessagesConstants.TaskNotFound,
                                                              request.Id));

        if (updatingTask.User.AuthId != authUserId)
        {
            throw new UnauthorizedAccessException(
                ErrorMessagesConstants.ForbiddenCreateTasksForAnotherUser);
        }

        _mapper.Map(request, updatingTask);
        _dbContext.Tasks.Update(updatingTask);

        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LoggingTemplates.TaskUpdated,
                               request.Id,
                               updatingTask.User.Id);
    }
}
