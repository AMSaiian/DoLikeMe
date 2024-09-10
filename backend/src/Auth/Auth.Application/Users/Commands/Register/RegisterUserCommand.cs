using Auth.Application.Common.Constants;
using Auth.Application.Common.Interfaces;
using Auth.Application.Common.Models.User;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Users.Commands.Register;

public record RegisterUserCommand : IRequest<Guid>
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public sealed class RegisterUserHandler(IAuthService authService,
                                        IMapper mapper,
                                        ILogger<RegisterUserHandler> logger)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userDto = _mapper.Map<UserDto>(request);
        Guid newUserId = await _authService.CreateUser(userDto, cancellationToken);

        logger.LogInformation(LoggingTemplates.UserRegistered, newUserId);

        return newUserId;
    }
}
