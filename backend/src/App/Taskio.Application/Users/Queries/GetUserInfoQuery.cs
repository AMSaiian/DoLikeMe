using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Domain.Entities;

namespace Taskio.Application.Users.Queries;

public record GetUserInfoQuery : IRequest<Guid>;

public class GetUserDataHandler(
    IAppDbContext dbContext,
    ICurrentUserService currentUser,
    ILogger<GetUserDataHandler> logger)
    : IRequestHandler<GetUserInfoQuery, Guid>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ILogger<GetUserDataHandler> _logger = logger;

    public async Task<Guid> Handle(GetUserInfoQuery request,
                                   CancellationToken cancellationToken)
    {
        Guid authId = _currentUser.GetUserIdOrThrow();

        User needUser = await _dbContext.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.AuthId == authId,
                                                 cancellationToken)
                     ?? throw new NotFoundException(
                            string
                                .Format(ErrorMessagesConstants.UserNotFound, authId));

        _logger.LogInformation(LoggingTemplates.UserRequestedInfo, authId);

        return needUser.Id;
    }
}
