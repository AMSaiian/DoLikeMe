using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Users.Commands.ChangePassword;

public record ChangePasswordCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string NewPasswordConfirmation { get; init; }
}

public class ChangePasswordHandler(IAuthService authService,
                                   IMapper mapper,
                                   ICurrentUserService currentUser,
                                   ILogger<ChangePasswordHandler> logger)
    : IRequestHandler<ChangePasswordCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ILogger<ChangePasswordHandler> _logger = logger;

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        Guid currentUserId = _currentUser.GetUserIdOrThrow();

        if (currentUserId != request.Id)
        {
            throw new ForbiddenAccessException(ErrorMessagesConstants.ForbiddenUpdateNotOwnedUser);
        }

        var updateUserPassword = _mapper.Map<UpdateUserPasswordDto>(request);

        await _authService.ChangePassword(updateUserPassword, cancellationToken);

        _logger.LogInformation(LoggingTemplates.UserPasswordUpdated, request.Id);
    }
}
