using Microsoft.AspNetCore.DataProtection;

using Haystac.Domain.Constants;

namespace Haystac.Console.Infrastructure.Services;

public class TokenService : ITokenService
{
    private static readonly string VAR_NAME = $"HAYSTAC_TOKEN";

    private readonly IDataProtectionProvider _rootProvider;

    public TokenService(IDataProtectionProvider protector)
    {
        _rootProvider = protector;
    }

    public Task<string?> GetTokenAsync()
    {
        var protector = _rootProvider.CreateProtector(Purposes.Authentication);

        var tokenData = Environment.GetEnvironmentVariable(VAR_NAME, EnvironmentVariableTarget.User);

        if (tokenData == null) return Task.FromResult<string?>(null);

        var token = protector.Unprotect(tokenData);
        
        return Task.FromResult<string?>(token);
    }

    public Task SetTokenAsync(string? token)
    {
        var protector = _rootProvider.CreateProtector(Purposes.Authentication);

        var tokenData = token == null ? null : protector.Protect(token);

        Environment.SetEnvironmentVariable(VAR_NAME, tokenData, EnvironmentVariableTarget.User);
        return Task.CompletedTask;
    }
}
