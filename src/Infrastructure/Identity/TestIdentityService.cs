namespace Haystac.Infrastructure.Identity;

public class TestIdentityService : IIdentityService
{
    private readonly IUserCollection<TestUser> _users;

    public TestIdentityService(IUserCollection<TestUser> users)
    {
        _users = users;
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _users.GetByUserIdAsync(userId);

        if (user == null) return false;

        return user.Policies.Contains(policyName);
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _users.GetByUserIdAsync(userId);

        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _users.GetByUserIdAsync(userId);

        if (user == null) return false;

        return user.Roles.Contains(role);
    }

    public Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    => throw new NotImplementedException();

    public Task<Result> DeleteUserAsync(string userId)
        => throw new NotImplementedException();

    public async Task<string?> GetClientIdAsync(string userId)
    {
        var user = await _users.GetByUserIdAsync(userId);

        if (user == null) return null;

        return user.ClientId;
    }
}
