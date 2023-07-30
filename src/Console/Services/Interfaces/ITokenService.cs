namespace Haystac.Console.Services;

public interface ITokenService
{
    public Task<string?> GetTokenAsync();

    public Task SetTokenAsync(string? token);
}
