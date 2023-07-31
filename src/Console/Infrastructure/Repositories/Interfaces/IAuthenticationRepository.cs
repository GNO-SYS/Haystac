namespace Haystac.Console.Infrastructure.Repositories;

public interface IAuthenticationRepository
{
    public Task<Result> LogInAsync(string username, string password);
    public Task LogOutAsync();
}