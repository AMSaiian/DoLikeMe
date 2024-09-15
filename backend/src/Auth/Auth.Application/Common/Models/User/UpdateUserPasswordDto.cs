namespace Auth.Application.Common.Models.User;

public record UpdateUserPasswordDto
{
    public required Guid Id { get; init; }
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
    public required string NewPasswordConfirmation { get; init; }
}
