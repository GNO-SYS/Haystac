using Microsoft.IdentityModel.Tokens;

namespace Haystac.Infrastructure.Identity;

public static class SecurityKeyHelper
{
    public static SymmetricSecurityKey GetSecurityKey(string secret)
        => new(Encoding.UTF8.GetBytes(secret.PadRight(128, '.')));

    public static SigningCredentials GetSigningCredentials(string secret)
        => new(GetSecurityKey(secret), SecurityAlgorithms.HmacSha256);
}
