using AMSaiian.Shared.Application.Models.Pagination;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public sealed class PageContextValidator : AbstractValidator<PageContext>
{
    public PageContextValidator()
    {
        RuleFor(context => context.PageNumber)
            .GreaterThan(0);

        RuleFor(context => context.PageSize)
            .GreaterThan(0);
    }
}
