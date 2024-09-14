using AMSaiian.Shared.Application.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Taskio.Application.Common.Constants;
using Taskio.Application.Tasks.Commands.Delete;
using Taskio.UnitTests.Common;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Taskio.UnitTests.Tests.Tasks.Application.Commands;

[Collection(UnitTestsCollectionDefinition.CollectionName)]
public class DeleteTaskHandlerTests(UnitTestFixture fixture)
{
    private readonly UnitTestFixture _fixture = fixture;
    private readonly ILogger<DeleteTaskHandler> _loggerMock = Mock.Of<ILogger<DeleteTaskHandler>>();

    [Fact]
    public async Task DeleteTaskHandlerWorksInAppropriateWay()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task deletingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DeleteTaskCommand command = new()
        {
            Id = deletingTask.Id
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[0]);

        DeleteTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _loggerMock);

        // Act
        await handler.Handle(command, CancellationToken.None);

        Domain.Entities.Task? updatedTask = await _fixture.DbContext.Tasks
            .FirstOrDefaultAsync(task => task.Id == command.Id);

        // Assert
        updatedTask
            .Should()
            .BeNull();
    }

    [Fact]
    public async Task DeleteTaskHandlerForbiddenForNotAuthorizedUsers()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task deletingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DeleteTaskCommand command = new()
        {
            Id = deletingTask.Id
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Throws(new UnauthorizedAccessException());

        DeleteTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _loggerMock);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task DeleteTaskHandlerForbiddenDeleteNotOwnedTask()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task deletingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DeleteTaskCommand command = new()
        {
            Id = deletingTask.Id
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(Guid.NewGuid());

        DeleteTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _loggerMock);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenAccessException>()
            .WithMessage(ErrorMessagesConstants.ForbiddenDeleteNotOwnedTask);
    }

    [Fact]
    public async Task DeleteTaskHandlerHandleCaseWhenTaskDoesNotExist()
    {
        // Arranging
        await _fixture.SetupDbContext();

        DeleteTaskCommand command = new()
        {
            Id = Guid.NewGuid()
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[0]);

        DeleteTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _loggerMock);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(string.Format(ErrorMessagesConstants.TaskNotFound, command.Id));
    }
}
