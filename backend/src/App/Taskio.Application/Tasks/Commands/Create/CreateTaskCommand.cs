using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Domain.Entities;
using Taskio.Domain.Enums;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Application.Tasks.Commands.Create;

public record CreateTaskCommand : IRequest<Guid>
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public required Status Status { get; init; }
    public required Priority Priority { get; init; }
    public DateTime? DueDate { get; init; }
    public required Guid UserId { get; init; }
}

public sealed class CreateTaskHandler(
    IAppDbContext dbContext,
    IMapper mapper,
    ICurrentUserService currentUserService,
    ILogger<CreateTaskHandler> logger)
    : IRequestHandler<CreateTaskCommand, Guid>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ILogger<CreateTaskHandler> _logger = logger;

    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        Guid authUserId = _currentUserService
            .GetUserIdOrThrow();

        User localUser = await _dbContext.Users
                             .FirstOrDefaultAsync(u => u.AuthId == authUserId,
                                                  cancellationToken)
                      ?? throw new NotFoundException(
                             string.Format(
                                 ErrorMessagesConstants.UserNotFound,
                                 authUserId));

        if (localUser.Id != request.UserId)
        {
            throw new ForbiddenAccessException(
                ErrorMessagesConstants.ForbiddenCreateTasksForAnotherUser);
        }

        var newTask = _mapper.Map<Task>(request);
        newTask.User = localUser;

        await _dbContext.Tasks.AddAsync(newTask, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(LoggingTemplates.TaskCreated,
                               newTask.Id,
                               localUser.Id);

        return newTask.Id;
    }
}
