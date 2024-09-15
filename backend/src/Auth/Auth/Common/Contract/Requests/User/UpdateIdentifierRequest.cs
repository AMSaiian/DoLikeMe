namespace Auth.Common.Contract.Requests.User;

public record UpdateIdentifierRequest
{
    private readonly string? _newName;
    private readonly string? _newEmail;

    public string? NewName
    {
        get => _newName;
        init => _newName = value?.Trim();
    }

    public string? NewEmail
    {
        get => _newEmail;
        init => _newEmail = value?.Trim();
    }
}
