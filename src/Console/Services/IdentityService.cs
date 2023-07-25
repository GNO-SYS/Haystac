using Haystac.Application.Common.Interfaces;

namespace Haystac.Console.Services;

public class IdentityService : IIdentityService
{
    private readonly IUser _user;

    public IdentityService(IUser user)
    {
        _user = user;
    }

    public Task<bool> AuthorizeAsync(string userId, string policyName)
        => Task.FromResult(true);

    public async Task<string?> GetUserNameAsync(string userId)
        => await _user.GetIdAsync();

    public Task<bool> IsInRoleAsync(string userId, string role)
        => Task.FromResult(true);

    public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        => throw new NotImplementedException();

    public Task<Result> DeleteUserAsync(string userId)
        => throw new NotImplementedException();
}
