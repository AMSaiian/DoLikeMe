using AMSaiian.Shared.Application.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Taskio.Application.Common.Constants;
using Taskio.Application.Tasks.Commands.Update;
using Taskio.Domain.Enums;
using Taskio.UnitTests.Common;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Taskio.UnitTests.Tests.Tasks.Application.Commands;

[Collection(UnitTestsCollectionDefinition.CollectionName)]
public class UpdateTaskHandlerTests(UnitTestFixture fixture)
{
    private readonly UnitTestFixture _fixture = fixture;
    private readonly ILogger<UpdateTaskHandler> _loggerMock = Mock.Of<ILogger<UpdateTaskHandler>>();

    [Fact]
    public async Task UpdateTaskHandlerChangesEntityStateInAppropriateWay()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task updatingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DateTime testDate = DateTime.UtcNow;
        UpdateTaskCommand command = new()
        {
            Id = updatingTask.Id,
            Description = "Updated Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending
        };

        Domain.Entities.Task expectedTask = new()
        {
            Id = updatingTask.Id,
            CreatedAt = updatingTask.CreatedAt,
            Priority = updatingTask.Priority,
            UserId = updatingTask.UserId,
            Title = updatingTask.Title,
            Description = command.Description,
            DueDate = command.DueDate,
            Status = command.Status!.Value,
            UpdatedAt = testDate,
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[0]);

        UpdateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _fixture.Mapper,
                                        _loggerMock);

        // Act
        await handler.Handle(command, CancellationToken.None);

        Domain.Entities.Task? updatedTask = await _fixture.DbContext.Tasks
            .FirstOrDefaultAsync(task => task.Title == expectedTask.Title);

        // Assert
        updatedTask
            .Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedTask,
                            options => options
                                .Excluding(task => task.UpdatedAt)
                                .Excluding(task => task.User))
            .And.Subject.As<Domain.Entities.Task>()
            .UpdatedAt
            .Should()
            .BeAfter(expectedTask.UpdatedAt.Value);
    }

    [Fact]
    public async Task UpdateTaskHandlerForbiddenForNotAuthorizedUsers()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task updatingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DateTime testDate = DateTime.UtcNow;
        UpdateTaskCommand command = new()
        {
            Id = updatingTask.Id,
            Description = "Updated Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Throws(new UnauthorizedAccessException());

        UpdateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _fixture.Mapper,
                                        _loggerMock);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task UpdateTaskHandlerForbiddenUpdateNotOwnedTask()
    {
        // Arranging
        await _fixture.SetupDbContext();

        Domain.Entities.Task updatingTask = await _fixture.DbContext.Tasks
            .FirstAsync(task => task.UserId == _fixture.AppDbContextInitializer.UserIds[0]);

        DateTime testDate = DateTime.UtcNow;
        UpdateTaskCommand command = new()
        {
            Id = updatingTask.Id,
            Description = "Updated Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(Guid.NewGuid());

        UpdateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _fixture.Mapper,
                                        _loggerMock);

        // Act
        Func<Task> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenAccessException>()
            .WithMessage(ErrorMessagesConstants.ForbiddenUpdateNotOwnedTask);
    }

    [Fact]
    public async Task UpdateTaskHandlerHandleCaseWhenTaskDoesNotExist()
    {
        // Arranging
        await _fixture.SetupDbContext();

        DateTime testDate = DateTime.UtcNow;
        UpdateTaskCommand command = new()
        {
            Id = Guid.NewGuid(),
            Description = "Updated Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[0]);

        UpdateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        _fixture.Mapper,
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
