namespace Haystac.Application.Common.Interfaces;

public interface IClientService
{
    Task<string?> GetClientIdAsync();

    Task<bool> IsCollectionVisible(Collection collec);

    Task<IEnumerable<Collection>> FilterCollectionsAsync(IEnumerable<Collection> collections);
}
