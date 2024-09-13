using System.Diagnostics.CodeAnalysis;

namespace Taskio.Common.Contract.Requests.User;

public record DeleteUserRequest
{
    private readonly string _password;
    private readonly string _confirmPassword;

    public required string Password
    {
        get => _password;
        [MemberNotNull(nameof(_password))]
        init => _password = value.Trim();
    }

    public required string ConfirmPassword
    {
        get => _confirmPassword;
        [MemberNotNull(nameof(_confirmPassword))]
        init => _confirmPassword = value.Trim();
    }
}
