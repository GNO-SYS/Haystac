using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Haystac.Infrastructure.Identity;

namespace Haystac.Infrastructure.Authentication;

public class TestAuthSettings
{
    public const string SectionName = "TestAuthSettings";

    public string Secret { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 30;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;

    public SymmetricSecurityKey SecurityKey
        => new(Encoding.UTF8.GetBytes(Secret));

    public SigningCredentials SigningCredentials
        => new(SecurityKey, SecurityAlgorithms.HmacSha256);
}

public class TestAuthenticationService : IAuthenticationService
{
    private readonly TestAuthSettings _settings;
    private readonly IUserCollection<TestUser> _users;

    public TestAuthenticationService(IOptions<TestAuthSettings> options,
                                     IUserCollection<TestUser> users)
    {
        _settings = options.Value;
        _users = users;
    }

    public async Task<string?> SignInAsync(string userName, string password, bool persist)
    {
        //< Retrieve user from internal list
        var user = await _users.GetByUserNameAsync(userName);

        //< Validate found & password matches
        if (user == null) return null;
        if (user.Password != password) return null;

        //< Generate & return access token
        return await GenerateTokenAsync(user);
    }

    Task<string> GenerateTokenAsync(TestUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var SecurityToken = new JwtSecurityToken(
                issuer: _settings.Issuer,
                expires: DateTime.Now.AddDays(_settings.ExpiryMinutes),
                audience: _settings.Audience,
                claims: claims,
                signingCredentials: _settings.SigningCredentials
            );

        var payload = new JwtSecurityTokenHandler().WriteToken(SecurityToken);

        return Task.FromResult(payload);
    }
}
