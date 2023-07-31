namespace Haystac.Console.Infrastructure.Repositories;

public interface ICollectionRepository
{
    public Task<Guid> CreateCollectionAsync(CollectionDto dto);
    public Task<List<CollectionDto>> GetAllCollectionsAsync();
    public Task<CollectionDto> GetCollectionByIdAsync(string collectionId);
    public Task UpdateCollectionAsync(CollectionDto dto);
    public Task DeleteCollectionAsync(string collectionId);
}
