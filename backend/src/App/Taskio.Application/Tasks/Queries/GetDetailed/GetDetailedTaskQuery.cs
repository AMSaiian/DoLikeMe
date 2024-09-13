using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Application.Common.Models.Task;

namespace Taskio.Application.Tasks.Queries.GetDetailed;

public record GetDetailedTaskQuery : IRequest<TaskFullDto>
{
    public required Guid Id { get; init; }
}

public class GetDetailedTaskHandler(
    ICurrentUserService currentUser,
    IAppDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<GetDetailedTaskQuery, TaskFullDto>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<TaskFullDto> Handle(GetDetailedTaskQuery request, CancellationToken cancellationToken)
    {
        Guid authUserId = _currentUser.GetUserIdOrThrow();

        Domain.Entities.Task task = await _dbContext.Tasks
                                        .AsNoTracking()
                                        .Include(t => t.User)
                                        .FirstOrDefaultAsync(t => t.Id == request.Id,
                                                             cancellationToken)
                                 ?? throw new NotFoundException(
                                        string.Format(ErrorMessagesConstants.TaskNotFound,
                                                      request.Id));

        if (task.User.AuthId != authUserId)
        {
            throw new ForbiddenAccessException(
                ErrorMessagesConstants.ForbiddenAccessNotOwnedResource);
        }

        TaskFullDto taskFullDto = _mapper.Map<TaskFullDto>(task);

        return taskFullDto;
    }
}
