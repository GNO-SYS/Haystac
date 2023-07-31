using Haystac.Application.Authentication.Commands;

using Haystac.Console.Infrastructure.Services;

namespace Haystac.Console.Infrastructure.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly HttpClient _client;
    private readonly ITokenService _token;

    public AuthenticationRepository(HttpClient client, ITokenService token)
    {
        _client = client;
        _token = token;
    }

    public async Task<Result> LogInAsync(string username, string password)
    {
        var cmd = new PasswordSignInCommand
        {
            UserName = username,
            Password = password
        };

        var json = JsonSerializer.Serialize(cmd);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var route = @"auth/signin";
        var response = await _client.PostAsync(route, requestContent);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure(new[] { responseBody });
        }

        await _token.SetTokenAsync(responseBody);

        return Result.Success();
    }

    public async Task LogOutAsync() => await _token.SetTokenAsync(null);
}
