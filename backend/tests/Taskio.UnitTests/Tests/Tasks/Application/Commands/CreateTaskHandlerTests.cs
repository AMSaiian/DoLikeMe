using AMSaiian.Shared.Application.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Taskio.Application.Common.Constants;
using Taskio.Application.Tasks.Commands.Create;
using Taskio.Domain.Enums;
using Taskio.UnitTests.Common;
using Task = System.Threading.Tasks.Task;
using UnauthorizedAccessException = AMSaiian.Shared.Application.Exceptions.UnauthorizedAccessException;

namespace Taskio.UnitTests.Tests.Tasks.Application.Commands;

[Collection(UnitTestsCollectionDefinition.CollectionName)]
public class CreateTaskHandlerTests(UnitTestFixture fixture)
{
    private readonly UnitTestFixture _fixture = fixture;

    [Fact]
    public async Task CreateTaskHandlerCreatesNewEntity()
    {
        // Arranging
        await _fixture.SetupDbContext();

        DateTime testDate = DateTime.UtcNow;
        CreateTaskCommand command = new()
        {
            Priority = Priority.Medium,
            Description = "Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending,
            Title = "Test Title",
            UserId = _fixture.AppDbContextInitializer.UserIds[0]
        };

        Domain.Entities.Task expectedTask = new()
        {
            Priority = Priority.Medium,
            Description = "Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending,
            Title = "Test Title",
            UserId = _fixture.AppDbContextInitializer.UserIds[0],
            CreatedAt = DateTime.UtcNow
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[0]);

        ILogger<CreateTaskHandler> logger = Mock.Of<ILogger<CreateTaskHandler>>();

        CreateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.Mapper,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        logger);

        // Act
        await handler.Handle(command, CancellationToken.None);

        Domain.Entities.Task? newTask = await _fixture.DbContext.Tasks
            .FirstOrDefaultAsync(task => task.Title == expectedTask.Title);

        // Assert
        newTask
            .Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedTask,
                            options => options
                                .Excluding(task => task.CreatedAt)
                                .Excluding(task => task.UpdatedAt)
                                .Excluding(task => task.Id)
                                .Excluding(task => task.User))
            .And.Subject.As<Domain.Entities.Task>()
            .CreatedAt
            .Should()
            .BeAfter(expectedTask.CreatedAt);
    }

    [Fact]
    public async Task CreateTaskHandlerForbiddenForNotAuthorizedUsers()
    {
        // Arranging
        await _fixture.SetupDbContext();

        DateTime testDate = DateTime.UtcNow;
        CreateTaskCommand command = new()
        {
            Priority = Priority.Medium,
            Description = "Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending,
            Title = "Test Title",
            UserId = _fixture.AppDbContextInitializer.UserIds[0]
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Throws(new UnauthorizedAccessException());

        ILogger<CreateTaskHandler> logger = Mock.Of<ILogger<CreateTaskHandler>>();

        CreateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.Mapper,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        logger);

        // Act
        Func<Task<Guid>> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task CreateTaskHandlerForAnotherUserIsForbidden()
    {
        // Arranging
        await _fixture.SetupDbContext();

        DateTime testDate = DateTime.UtcNow;
        CreateTaskCommand command = new()
        {
            Priority = Priority.Medium,
            Description = "Test Description",
            DueDate = testDate.AddDays(10),
            Status = Status.Pending,
            Title = "Test Title",
            UserId = _fixture.AppDbContextInitializer.UserIds[0]
        };

        _fixture.CurrentUserServiceMoq.Reset();
        _fixture.CurrentUserServiceMoq
            .Setup(service => service.GetUserIdOrThrow())
            .Returns(_fixture.AppDbContextInitializer.AuthIds[1]);

        ILogger<CreateTaskHandler> logger = Mock.Of<ILogger<CreateTaskHandler>>();

        CreateTaskHandler handler = new(_fixture.DbContext,
                                        _fixture.Mapper,
                                        _fixture.CurrentUserServiceMoq.Object,
                                        logger);

        // Act
        Func<Task<Guid>> act = () => handler.Handle(command, CancellationToken.None);

        // Assert
        await act
            .Should()
            .ThrowAsync<ForbiddenAccessException>()
            .WithMessage(ErrorMessagesConstants.ForbiddenCreateTasksForAnotherUser);
    }
}
