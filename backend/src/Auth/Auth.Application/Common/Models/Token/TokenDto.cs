namespace Auth.Application.Common.Models.Token;

public record TokenDto
{
    public required string Value { get; init; }
}
