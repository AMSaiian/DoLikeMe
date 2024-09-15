using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Infrastructure.Interceptors;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Taskio.Application.Common.Mapping;
using Taskio.Infrastructure.Persistence;
using Taskio.Infrastructure.Persistence.Seeding.Fakers;
using Taskio.Infrastructure.Persistence.Seeding.Initializers;

namespace Taskio.UnitTests.Common;

public sealed class UnitTestFixture
{
    public UnitTestFixture()
    {
        DbContext = BuildDbContext();

        AppDbContextInitializer = BuildAppDbContextInitializer(DbContext);

        var config = new MapperConfiguration(
            cfg =>
                cfg.AddProfiles(
                [
                    new Taskio.Application.Common.Mapping.UserProfile(),
                    new Auth.Application.Common.Mapping.UserProfile(),
                    new TaskProfile()
                ]));

        Mapper = config.CreateMapper();
        CurrentUserServiceMoq = new Mock<ICurrentUserService>();
    }

    public AppDbContext DbContext { get; }

    public IAppDbContextInitializer AppDbContextInitializer { get; }

    public IMapper Mapper { get; }

    public Mock<ICurrentUserService> CurrentUserServiceMoq { get; }

    public async Task SetupDbContext()
    {
        await AppDbContextInitializer.ClearStorageAsync();
        await AppDbContextInitializer.SeedAsync();
    }

    private AppDbContext BuildDbContext()
    {
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .AddInterceptors(new AuditedInterceptor())
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
    }

    private AppDbContextInitializer BuildAppDbContextInitializer(AppDbContext context)
    {
        return new AppDbContextInitializer(Mock.Of<ILogger<AppDbContextInitializer>>(),
                                           context,
                                           new TaskFaker());
    }
}

[CollectionDefinition(CollectionName)]
public class UnitTestsCollectionDefinition : ICollectionFixture<UnitTestFixture>
{
    public const string CollectionName = "UnitTestsWithObviousDependenciesCollection";
}
