using AMSaiian.Shared.Application.Models.Pagination;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public sealed class PaginationContextValidator : AbstractValidator<PaginationContext>
{
    public PaginationContextValidator(IValidator<OrderContext> orderContextValidator,
                                      IValidator<PageContext> pageContextValidator)
    {
        RuleFor(context => context.PageContext)
            .NotNull()
            .SetValidator(pageContextValidator);

        RuleFor(context => context.OrderContext)
            .NotNull()
            .SetValidator(orderContextValidator);
    }
}
