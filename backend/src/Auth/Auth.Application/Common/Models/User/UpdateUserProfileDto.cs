namespace Auth.Application.Common.Models.User;

public record UpdateUserProfileDto
{
    public required Guid Id { get; init; }
    public string? NewName { get; init; }
    public string? NewEmail { get; init; }
}
