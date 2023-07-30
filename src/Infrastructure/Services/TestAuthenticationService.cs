using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Haystac.Domain.Constants;

namespace Haystac.Infrastructure.Services;

public class TestAuthSettings
{
    public const string SectionName = "TestAuthSettings";

    public string Secret { get; init; } = string.Empty;
    public int ExpiryMinutes { get; init; } = 30;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;

    public SymmetricSecurityKey SecurityKey 
        => new (Encoding.UTF8.GetBytes(Secret));

    public SigningCredentials SigningCredentials 
        => new (SecurityKey, SecurityAlgorithms.HmacSha256);
}

record class TestUser
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public ICollection<string> Roles { get; set; } = new HashSet<string>();

    public ICollection<string> Policies { get; set; } = new HashSet<string>();
}

public class TestAuthenticationService : IAuthenticationService
{
    private readonly TestAuthSettings _settings;

    private readonly ICollection<TestUser> _users = new HashSet<TestUser>
    {
        new TestUser
        {
            UserName = "admin",
            Password = "testAdmin",
            Roles = new HashSet<string> { Roles.Administrator },
            Policies = new HashSet<string> { Policies.CanEditClients }
        },
        new TestUser
        {
            UserName = "user",
            Password = "testUser"
            //< TODO - Test 'ClientId' field here for filtering results
        }
    };

    public TestAuthenticationService(IOptions<TestAuthSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<string?> SignInAsync(string userName, string password, bool persist)
    {
        //< Retrieve user from internal list
        var user = GetUserByName(userName);

        //< Validate found & password matches
        if (user == null) return null;
        if (user.Password != password) return null;

        //< Generate & return access token
        return await GenerateTokenAsync(user);
    }

    TestUser? GetUserByName(string userName)
        => _users.FirstOrDefault(u => u.UserName == userName);

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
