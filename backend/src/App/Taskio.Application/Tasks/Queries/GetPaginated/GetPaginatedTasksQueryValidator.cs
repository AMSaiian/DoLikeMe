using AMSaiian.Shared.Application.Models.Filtering;
using AMSaiian.Shared.Application.Models.Pagination;
using FluentValidation;

namespace Taskio.Application.Tasks.Queries.GetPaginated;

public class GetPaginatedTasksQueryValidator
    : AbstractValidator<GetPaginatedTasksQuery>
{
    public GetPaginatedTasksQueryValidator(IValidator<FilterContext> filterValidator,
                                           IValidator<PaginationContext> paginationValidator,
                                           IValidator<RangeContext> rangingValidator)
    {
        RuleFor(query => query.UserId)
            .NotEmpty();

        RuleFor(query => query.PaginationContext)
            .NotNull()
            .SetValidator(paginationValidator);

        RuleFor(query => query.FilterContext)
            .SetValidator(filterValidator)
            .When(query => query.FilterContext is not null);

        RuleFor(query => query.RangeContext)
            .SetValidator(rangingValidator)
            .When(query => query.RangeContext is not null);
    }
}
