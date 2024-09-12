using FluentValidation;

namespace Taskio.Application.Tasks.Queries.GetDetailed;

public class GetDetailedTaskQueryValidator : AbstractValidator<GetDetailedTaskQuery>
{
    public GetDetailedTaskQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty();
    }
}
