using System.Diagnostics.CodeAnalysis;

namespace Taskio.Common.Contract.Requests.User;

public record CreateUserRequest
{
    private readonly string _name;
    private readonly string _email;
    private readonly string _password;
    private readonly string _confirmPassword;

    public required string Name
    {
        get => _name;
        [MemberNotNull(nameof(_name))] init => _name = value.Trim();
    }

    public required string Email
    {
        get => _email;
        [MemberNotNull(nameof(_email))] init => _email = value.Trim();
    }

    public required string Password
    {
        get => _password;
        [MemberNotNull(nameof(_password))] init => _password = value.Trim();
    }

    public required string ConfirmPassword
    {
        get => _confirmPassword;
        [MemberNotNull(nameof(_confirmPassword))]
        init => _confirmPassword = value.Trim();
    }
}
