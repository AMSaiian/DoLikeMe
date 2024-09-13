using FluentValidation;

namespace Taskio.Application.Tasks.Queries.GetDetailed;

public sealed class GetDetailedTaskQueryValidator : AbstractValidator<GetDetailedTaskQuery>
{
    public GetDetailedTaskQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}
