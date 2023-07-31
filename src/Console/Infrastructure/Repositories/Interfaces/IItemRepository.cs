namespace Haystac.Console.Infrastructure.Repositories;

public interface IItemRepository
{
    public Task<Guid> CreateItemAsync(ItemDto dto);
    public Task<ItemDto> GetItemByIdAsync(string collectionId, string id);
    public Task UpdateItemAsync(ItemDto dto);
    public Task DeleteItemAsync(string collectionId, string id);
}