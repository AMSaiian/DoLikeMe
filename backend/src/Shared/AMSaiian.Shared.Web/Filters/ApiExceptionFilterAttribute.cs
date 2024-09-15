using AMSaiian.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UnauthorizedAccessException = AMSaiian.Shared.Application.Exceptions.UnauthorizedAccessException;

namespace AMSaiian.Shared.Web.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    protected readonly IDictionary<Type, Action<Exception, ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<Exception, ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
            { typeof(ConflictException), HandleConflictException },
            { typeof(UnprocessableException), HandleUnprocessableException },
            { typeof(FormatException), HandleFormatException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    protected virtual void HandleException(ExceptionContext context)
    {
        Exception? exception = context.Exception;

        if (_exceptionHandlers.TryGetValue(exception.GetType(),
                                           out Action<Exception, ExceptionContext>? handler)
         || ((exception = context.Exception.InnerException) is not null
          && _exceptionHandlers.TryGetValue(exception.GetType(),
                                            out handler)))
        {
            handler(exception, context);
            context.ExceptionHandled = true;
        }
    }

    protected virtual void HandleValidationException(Exception exception, ExceptionContext context)
    {
        var castedException = (ValidationException)exception;

        var details = new ValidationProblemDetails(castedException.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
    }

    protected virtual void HandleNotFoundException(Exception exception, ExceptionContext context)
    {
        var castedException = (NotFoundException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = castedException.Message
        };

        context.Result = new NotFoundObjectResult(details);
    }

    protected virtual void HandleUnauthorizedAccessException(Exception exception, ExceptionContext context)
    {
        var castedException = (UnauthorizedAccessException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            Detail = castedException.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status401Unauthorized
        };
    }

    protected virtual void HandleForbiddenAccessException(Exception exception, ExceptionContext context)
    {
        var castedException = (ForbiddenAccessException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            Detail = castedException.Message
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }

    protected virtual void HandleConflictException(Exception exception, ExceptionContext context)
    {
        var castedException = (ConflictException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict occured during processing request",
            Detail = castedException.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status409Conflict
        };
    }

    protected virtual void HandleUnprocessableException(Exception exception, ExceptionContext context)
    {
        var castedException = (UnprocessableException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Request has been recognized but can't be processed",
            Detail = castedException.Message,
            Type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity
        };
    }

    protected virtual void HandleFormatException(Exception exception, ExceptionContext context)
    {
        var castedException = (FormatException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Provided in request data is not valid",
            Detail = castedException.Message,
            Type = "https://www.rfc-editor.org/rfc/rfc4918#section-11.2"
        };

        context.Result = new BadRequestObjectResult(details);
    }
}
