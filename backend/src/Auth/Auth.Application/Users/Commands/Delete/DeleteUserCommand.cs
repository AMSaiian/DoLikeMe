using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;
using Auth.Application.Users.Commands.UpdateProfileInfo;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Users.Commands.Delete;

public record DeleteUserCommand : IRequest
{
    public required Guid Id { get; init; }
    public required string Password { get; init; }
    public required string ConfirmPassword { get; init; }
}

public class DeleteUserHandler(IAuthService authService,
                               IMapper mapper,
                               ICurrentUserService currentUser,
                               ILogger<UpdateProfileInfoHandler> logger)
    : IRequestHandler<DeleteUserCommand>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly ILogger<UpdateProfileInfoHandler> _logger = logger;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        Guid userId = _currentUser.GetUserIdOrThrow();

        if (request.Id != userId)
        {
            throw new ForbiddenAccessException(ErrorMessagesConstants.ForbiddenDeleteNotOwnedUser);
        }

        var deleteUserDto = _mapper.Map<DeleteUserDto>(request);

        await _authService.DeleteUser(deleteUserDto, cancellationToken);

        _logger.LogInformation(LoggingTemplates.UserDeleted, request.Id);
    }
}
