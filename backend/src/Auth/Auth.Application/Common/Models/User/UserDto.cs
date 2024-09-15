namespace Auth.Application.Common.Models.User;

public record UserDto
{
    public required string Name { get; init; }
    public required string Password { get; init; }
    public required string Email { get; init; }
}
