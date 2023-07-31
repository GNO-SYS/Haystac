using Haystac.Domain.Constants;

namespace Haystac.Console.Infrastructure.Services;

public class TokenService : ITokenService
{
    private static readonly string VAR_NAME = $"HAYSTAC_TOKEN";

    public Task<string?> GetTokenAsync()
    {
        var token = Environment.GetEnvironmentVariable(VAR_NAME, EnvironmentVariableTarget.User);

        return Task.FromResult(token);
    }

    public Task SetTokenAsync(string? token)
    {
        Environment.SetEnvironmentVariable(VAR_NAME, token, EnvironmentVariableTarget.User);

        return Task.CompletedTask;
    }
}
