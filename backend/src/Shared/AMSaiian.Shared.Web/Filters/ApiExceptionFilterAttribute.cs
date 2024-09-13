using AMSaiian.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UnauthorizedAccessException = AMSaiian.Shared.Application.Exceptions.UnauthorizedAccessException;

namespace AMSaiian.Shared.Web.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    protected readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(ConflictException), HandleConflictException },
            { typeof(UnprocessableException), HandleUnprocessableException },
        };
    }


    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    protected virtual void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();

        if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext>? handler))
        {
            handler(context);
        }
    }

    protected virtual void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;

        var details = new ValidationProblemDetails(exception.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    protected virtual void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = exception.Message
        };

        context.Result = new NotFoundObjectResult(details);

        context.ExceptionHandled = true;
    }

    protected virtual void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var exception = (UnauthorizedAccessException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };

        context.ExceptionHandled = true;
    }

    protected virtual void HandleForbiddenAccessException(ExceptionContext context)
    {
        var exception = (ForbiddenAccessException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Detail = exception.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };

        context.ExceptionHandled = true;
    }

    protected virtual void HandleConflictException(ExceptionContext context)
    {
        var exception = (ConflictException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict occured during processing request",
            Detail = exception.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status409Conflict
        };

        context.ExceptionHandled = true;
    }

    protected virtual void HandleUnprocessableException(ExceptionContext context)
    {
        var exception = (UnprocessableException)context.Exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Request has been recognized but can't be processed",
            Detail = exception.Message,
            Type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity
        };

        context.ExceptionHandled = true;
    }
}
