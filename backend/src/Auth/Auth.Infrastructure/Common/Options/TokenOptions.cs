using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Common.Options;

public class TokenProviderOptions
{
    public const string SectionName = "TokenProvider";

    public string Audience { get; set; } = default!;

    public string Issuer { get; set; } = default!;

    public int ExpiresInMinutes { get; set; }

    public string Secret
    {
        get => _secret;
        set
        {
            _parsedSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value));
            _secret = value;
        }
    }

    public SymmetricSecurityKey GetParsedSecret()
    {
        return _parsedSecret;
    }

    private SymmetricSecurityKey _parsedSecret = default!;
    private string _secret = default!;
}
