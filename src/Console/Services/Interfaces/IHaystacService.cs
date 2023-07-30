namespace Haystac.Console.Services;

public interface IHaystacService
{
    public Task<Result> LogInAsync(string username, string password);

    public Task LogOutAsync();
}
