namespace Haystac.Application.Common.Interfaces;

public interface IClientService
{
    Task<string?> GetClientIdAsync();

    Task<IEnumerable<Collection>> FilterCollectionsAsync(IEnumerable<Collection> collections);
}
