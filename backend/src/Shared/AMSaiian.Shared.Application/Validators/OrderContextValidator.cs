using AMSaiian.Shared.Application.Models.Pagination;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public sealed class OrderContextValidator : AbstractValidator<OrderContext>
{
    public OrderContextValidator()
    {
        RuleFor(context => context.PropertyName)
            .NotEmpty();
    }
}
