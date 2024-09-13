using AMSaiian.Shared.Application.Exceptions;
using AMSaiian.Shared.Application.Interfaces;
using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Application.Models.Pagination;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskio.Application.Common.Constants;
using Taskio.Application.Common.Interfaces;
using Taskio.Application.Common.Models.Task;
using Taskio.Domain.Entities;
using Task = Taskio.Domain.Entities.Task;

namespace Taskio.Application.Tasks.Queries.GetPaginated;

public record GetPaginatedTasksQuery : IRequest<Paginated<TaskShortDto>>
{
    public required Guid UserId { get; init; }
    public required PaginationContext PaginationContext { get; init; }
    public FilterContext? FilterContext { get; init; }
    public RangeContext? RangeContext { get; init; }
}

public class GetPaginatedTasksHandler(
    ICurrentUserService currentUser,
    IAppDbContext dbContext,
    IMapper mapper,
    IPaginationService paginationService,
    IFilterFactory filterFactory,
    IRangeFactory rangeFactory)
    : IRequestHandler<GetPaginatedTasksQuery, Paginated<TaskShortDto>>
{
    private readonly ICurrentUserService _currentUser = currentUser;
    private readonly IAppDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;
    private readonly IPaginationService _paginationService = paginationService;
    private readonly IFilterFactory _filterFactory = filterFactory;
    private readonly IRangeFactory _rangeFactory = rangeFactory;

    public async Task<Paginated<TaskShortDto>> Handle(GetPaginatedTasksQuery request,
                                                      CancellationToken cancellationToken)
    {
        Guid authUserId = _currentUser.GetUserIdOrThrow();

        User user = await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.Id == request.UserId,
                                             cancellationToken)
                 ?? throw new NotFoundException(
                        string.Format(ErrorMessagesConstants.UserNotFound,
                                      request.UserId));

        if (authUserId != user.AuthId)
        {
            throw new ForbiddenAccessException(
                ErrorMessagesConstants.ForbiddenAccessNotOwnedResource);
        }

        var tasksQuery = _dbContext.Users
            .Entry(user)
            .Collection(u => u.Tasks)
            .Query()
            .AsNoTracking();

        tasksQuery = request.FilterContext is not null
            ? _filterFactory.FilterDynamically(tasksQuery, request.FilterContext)
            : tasksQuery;

        tasksQuery = request.RangeContext is not null
            ? _rangeFactory.RangeDynamically(tasksQuery, request.RangeContext)
            : tasksQuery;

        Paginated<Task> paginatedEntities = await _paginationService
            .PaginateAsync(tasksQuery,
                           request.PaginationContext,
                           cancellationToken);

        Paginated<TaskShortDto> paginatedDtos = new()
        {
            PaginationInfo = paginatedEntities.PaginationInfo,
            Records = _mapper.Map<List<TaskShortDto>>(paginatedEntities.Records)
        };

        return paginatedDtos;
    }
}
