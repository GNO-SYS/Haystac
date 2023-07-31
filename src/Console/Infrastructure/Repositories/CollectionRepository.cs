namespace Haystac.Console.Infrastructure.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly HttpClient _client;

    public CollectionRepository(HttpClient client)
    {
        _client = client;
    }

    public Task<Guid> CreateCollectionAsync(CollectionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<CollectionDto> GetCollectionByIdAsync(string collectionId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateCollectionAsync(CollectionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCollectionAsync(string collectionId)
    {
        throw new NotImplementedException();
    }
}
