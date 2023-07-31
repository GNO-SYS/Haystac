using Haystac.Domain.Constants;

namespace Haystac.Infrastructure.Identity;

public class TestUserCollection : IUserCollection<TestUser>
{
    private readonly ICollection<TestUser> _users = new HashSet<TestUser>
    {
        new TestUser
        {
            UserName = "admin",
            Password = "testAdmin",
            Roles = new HashSet<string> { Roles.Administrator },
            Policies = new HashSet<string> { Policies.CanEditClients }
        },
        new TestUser
        {
            UserName = "user",
            Password = "testUser"
            //< TODO - Test 'ClientId' field here for filtering results
        }
    };

    public Task<TestUser?> GetByUserIdAsync(string userId)
    {
        var user = _users.FirstOrDefault(u => u.Id.ToString() == userId);

        return Task.FromResult(user);
    }

    public Task<TestUser?> GetByUserNameAsync(string userName)
    {
        var user = _users.FirstOrDefault(u => u.UserName == userName);

        return Task.FromResult(user);
    }
}
