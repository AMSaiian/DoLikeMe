namespace Auth.Common.Contract.Requests.User;

public record LoginRequest
{
    private readonly string? _identifier;
    private readonly string? _password;

    public string? Identifier
    {
        get => _identifier;
        init => _identifier = value?.Trim();
    }

    public string? Password
    {
        get => _password;
        init => _password = value?.Trim();
    }
}
