namespace Auth.Application.Common.Models.User;

public record SignUpDto
{
    public required string Identifier { get; init; }
    public required string Password { get; init; }
}
