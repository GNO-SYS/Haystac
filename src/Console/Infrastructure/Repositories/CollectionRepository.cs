namespace Haystac.Console.Infrastructure.Repositories;

public class CollectionRepository : ICollectionRepository
{
    private readonly HttpClient _client;

    public CollectionRepository(HttpClient client)
    {
        _client = client;
    }

    public async Task<Guid> CreateCollectionAsync(CollectionDto dto, bool isAnonymous = false)
    {
        var json = JsonSerializer.Serialize(dto);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var route = $"{dto.Identifier}?anonymous={isAnonymous}";
        var response = await _client.PostAsync(route, requestContent);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create Collection due to: {responseBody}");
        }

        return JsonSerializer.Deserialize<Guid>(responseBody);
    }

    public async Task<List<CollectionDto>> GetAllCollectionsAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "");

        var response = await _client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        var dtos = JsonSerializer.Deserialize<List<CollectionDto>>(responseBody);
        if (dtos == null) throw new Exception($"Failed to deserialize response: {responseBody}");

        return dtos;
    }

    public async Task<CollectionDto> GetCollectionByIdAsync(string collectionId)
    {
        var route = $"{collectionId}";
        var request = new HttpRequestMessage(HttpMethod.Get, route);

        var response = await _client.SendAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        var dto = JsonSerializer.Deserialize<CollectionDto>(responseBody);
        if (dto == null) throw new Exception($"Failed to deserialize response: {responseBody}");

        return dto;
    }

    public async Task UpdateCollectionAsync(CollectionDto dto)
    {
        var json = JsonSerializer.Serialize(dto);
        var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

        var route = $"{dto.Identifier}";
        var response = await _client.PutAsync(route, requestContent);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"ERROR: [{response.StatusCode}] {responseBody ?? ""}");
        }

        return;
    }

    public async Task DeleteCollectionAsync(string collectionId)
    {
        var route = $"{collectionId}";
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
