using System.Diagnostics.CodeAnalysis;

namespace Auth.Common.Contract.Requests.User;

public record ChangePasswordRequest
{
    private readonly string _oldPassword;
    private readonly string _newPassword;
    private readonly string _newPasswordConfirmation;

    public required string OldPassword
    {
        get => _oldPassword;
        [MemberNotNull(nameof(_oldPassword))] init => _oldPassword = value.Trim();
    }

    public required string NewPassword
    {
        get => _newPassword;
        [MemberNotNull(nameof(_newPassword))] init => _newPassword = value.Trim();
    }

    public required string NewPasswordConfirmation
    {
        get => _newPasswordConfirmation;
        [MemberNotNull(nameof(_newPasswordConfirmation))]
        init => _newPasswordConfirmation = value.Trim();
    }
}
