using AMSaiian.Shared.Application.Models;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public class FilterContextValidator : AbstractValidator<FilterContext>
{
    public FilterContextValidator()
    {
        RuleFor(context => context.PropertyName)
            .NotEmpty();

        RuleFor(context => context.Filters)
            .NotEmpty();
    }
}
