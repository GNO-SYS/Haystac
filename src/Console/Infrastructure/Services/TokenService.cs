using Haystac.Domain.Constants;

namespace Haystac.Console.Infrastructure.Services;

public class TokenService : ITokenService
{
    private static readonly string VAR_NAME = $"{Prefixes.ENV_PREFIX}__AccessToken";

    public Task<string?> GetTokenAsync()
    {
        var token = Environment.GetEnvironmentVariable(VAR_NAME);

        return Task.FromResult(token);
    }

    public Task SetTokenAsync(string? token)
    {
        Environment.SetEnvironmentVariable(VAR_NAME, token);

        return Task.CompletedTask;
    }
}
