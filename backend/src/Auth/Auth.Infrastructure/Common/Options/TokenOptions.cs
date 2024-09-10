namespace Auth.Infrastructure.Common.Options;

public class TokenProviderOptions
{
    public const string SectionName = "TokenProvider";

    public string Audience { get; set; } = default!;

    public string Issuer { get; set; } = default!;

    public string Secret { get; set; } = default!;

    public int ExpiresInMinutes { get; set; }
}
