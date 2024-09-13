using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Users.Commands.UpdateProfileInfo;

public record UpdateProfileInfoCommand : IRequest
{
    public required Guid Id { get; init; }
    public string? NewName { get; init; }
    public string? NewEmail { get; init; }
}

public sealed class UpdateProfileInfoHandler(
    IAuthService authService,
    IMapper mapper,
    ICurrentUserService currentUser,
    ILogger<UpdateProfileInfoHandler> logger)
    : IRequestHandler<UpdateProfileInfoCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ILogger<UpdateProfileInfoHandler> _logger = logger;

    public async Task Handle(UpdateProfileInfoCommand request, CancellationToken cancellationToken)
    {
        Guid currentUserId = _currentUser.GetUserIdOrThrow();

        if (currentUserId != request.Id)
        {
            throw new ForbiddenAccessException(ErrorMessagesConstants.ForbiddenUpdateNotOwnedUser);
        }

        var updateUserProfileDto = _mapper.Map<UpdateUserProfileDto>(request);

        await _authService.UpdateUser(updateUserProfileDto, cancellationToken);

        _logger.LogInformation(LoggingTemplates.UserProfileUpdated, request.Id);
    }
}
