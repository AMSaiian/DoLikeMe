using AMSaiian.Shared.Application.Models;
using FluentValidation;

namespace AMSaiian.Shared.Application.Validators;

public class OrderContextValidator : AbstractValidator<OrderContext>
{
    public OrderContextValidator()
    {
        RuleFor(context => context.PropertyName)
            .NotEmpty();
    }
}
