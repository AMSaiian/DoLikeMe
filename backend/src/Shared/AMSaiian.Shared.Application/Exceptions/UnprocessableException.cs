namespace AMSaiian.Shared.Application.Exceptions;

public class UnprocessableException : Exception
{
    public UnprocessableException()
        : base()
    {
    }

    public UnprocessableException(string message)
        : base(message)
    {
    }

    public UnprocessableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
