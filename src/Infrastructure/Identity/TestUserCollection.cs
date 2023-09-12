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
            Policies = new HashSet<string> { Policies.CanEditClients },
            ClientId = "d9ce7e72-5e03-4a62-ad4e-e15f793c6e72"
        },
        new TestUser
        {
            UserName = "user",
            Password = "testUser",
            ClientId = "4af8a081-be9a-4a43-b6dd-88fec0de9242"
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
