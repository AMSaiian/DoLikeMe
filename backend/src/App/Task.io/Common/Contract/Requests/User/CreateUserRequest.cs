namespace Task.io.Common.Contract.Requests.User;

public record CreateUserRequest
{
    private readonly string? _name;
    private readonly string? _email;
    private readonly string? _password;
    private readonly string? _confirmPassword;

    public string? Name
    {
        get => _name;
        init => _name = value?.Trim();
    }

    public string? Email
    {
        get => _email;
        init => _email = value?.Trim();
    }

    public string? Password
    {
        get => _password;
        init => _password = value?.Trim();
    }

    public string? ConfirmPassword
    {
        get => _confirmPassword;
        init => _confirmPassword = value?.Trim();
    }
}
