namespace Haystac.Console.Infrastructure.Repositories;

public class ItemRepository : IItemRepository
{
    private readonly HttpClient _client;

    public ItemRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<Guid> CreateItemAsync(ItemDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var route = $@"{dto.Collection}\items\{dto.Identifier}";
        var response = await _client.PostAsync(route, requestContent);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        return JsonSerializer.Deserialize<Guid>(responseBody);
    }

    public async Task<ItemDto> GetItemByIdAsync(string collectionId, string id)
    {
        var route = $@"{collectionId}\items\{id}";
        var request = new HttpRequestMessage(HttpMethod.Get, route);

        var response = await _client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        var dto = JsonSerializer.Deserialize<ItemDto>(responseBody);
        if (dto == null) throw new Exception($"Failed to deserialize response: {responseBody}");

        return dto;
    }

    public async Task UpdateItemAsync(ItemDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var route = $@"{dto.Collection}\items\{dto.Identifier}";
        var response = await _client.PutAsync(route, requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        return;
    }

    public async Task DeleteItemAsync(string collectionId, string id)
    {
        var route = $@"{collectionId}\items\{id}";
        var delRequest = new HttpRequestMessage(HttpMethod.Delete, route);

        var response = await _client.SendAsync(delRequest);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        return;
    }
}
