using Haystac.Domain.Constants;

namespace Haystac.Infrastructure.Services;

public class ClientService : IClientService
{
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public ClientService(
        IUser user,
        IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<string?> GetClientIdAsync()
    {
        if (_user.Id == null) return null;

        return await _identityService.GetClientIdAsync(_user.Id);
    }

    public async Task<bool> IsCollectionVisible(Collection collec)
    {
        if (_user.Id == null)
        {
            return collec.ClientId == null;
        }

        if (await _identityService.IsInRoleAsync(_user.Id, Roles.Administrator))
        {
            return true;
        }

        var clientId = await _identityService.GetClientIdAsync(_user.Id);

        if (clientId == null) return collec.ClientId == null;

        return collec.ClientId == clientId;
    }

    public async Task<IEnumerable<Collection>> FilterCollectionsAsync(IEnumerable<Collection> collections)
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
