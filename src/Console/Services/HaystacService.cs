using Haystac.Application.Authentication.Commands;

namespace Haystac.Console.Services;

public class HaystacService : IHaystacService
{
    private readonly string _loginUrl = "users/signin";

    private readonly HttpClient _client;
    private readonly ITokenService _token;

    public HaystacService(HttpClient httpClient, ITokenService token)
    {
        _client = httpClient;
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

        var response = await _client.PostAsync(_loginUrl, requestContent);
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
