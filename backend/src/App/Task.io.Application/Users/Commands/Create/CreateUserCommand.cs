﻿using AMSaiian.Shared.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task.io.Application.Common.Constants;
using Task.io.Application.Common.Interfaces;
using Task.io.Domain.Entities;

namespace Task.io.Application.Users.Commands.Create;

public record CreateUserCommand : IRequest<Guid>
{
    public required Guid AuthId { get; init; }
}

public class CreateUserCommandHandler(IAppDbContext dbContext,
                                      IMapper mapper,
                                      ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger = logger;

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        bool isLocalUserExists = await _dbContext.Users
            .AnyAsync(u => u.AuthId == request.AuthId,
                      cancellationToken);

        if (isLocalUserExists)
        {
            throw new ConflictException(ErrorMessagesConstants.IdentifierDataNotUnique);
        }

        var newUser = _mapper.Map<User>(request);

        await _dbContext.Users.AddAsync(newUser, cancellationToken);

        _logger.LogInformation(LoggingTemplates.UserCreated, newUser.Id);

        return newUser.Id;
    }
}