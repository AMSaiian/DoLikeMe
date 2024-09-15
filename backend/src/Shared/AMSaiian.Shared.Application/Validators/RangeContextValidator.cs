using AMSaiian.Shared.Application.Models.Filtering;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public sealed class RangeContextValidator : AbstractValidator<RangeContext>
{
    public RangeContextValidator()
    {
        RuleFor(context => context.PropertyName)
            .NotEmpty();

        RuleFor(context => context.Start)
            .NotEmpty();

        RuleFor(context => context.End)
            .NotEmpty();
    }
}
