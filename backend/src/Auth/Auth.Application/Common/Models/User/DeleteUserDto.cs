namespace Auth.Application.Common.Models.User;

public record DeleteUserDto
{
    public required Guid Id { get; init; }
    public required string Password { get; init; }
}
