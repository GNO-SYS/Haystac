using Haystac.Domain.Constants;

namespace Haystac.Infrastructure.Services;

public class ClientFilter : IClientFilter
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public ClientFilter(
        IUser user,
        IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<IEnumerable<Collection>> FilterAsync(IEnumerable<Collection> collections)
    {
        if (_user.Id == null)
        {
            //< User un-authenticated, only return 'anonymous' Collections
            return await GetAnonymousAsync(collections);
        }

        if (await _identityService.IsInRoleAsync(_user.Id, Roles.Administrator))
        {
            //< Administrators get access to all Collections, no filtration
            return collections;
        }

        var clientId = await _identityService.GetClientIdAsync(_user.Id);

        if (clientId == null)
        {
            //< No assigned ClientId, also get only 'anonymous' Collections
            return await GetAnonymousAsync(collections);
        }

        return collections.Where(c => c.ClientId == null || c.ClientId.Equals(clientId));
    }

    public static Task<IEnumerable<Collection>> GetAnonymousAsync(IEnumerable<Collection> collections)
        => Task.FromResult(collections.Where(c => c.ClientId == null));
}
