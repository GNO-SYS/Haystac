using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Common.Models;

public class CollectionListDto
{
    [JsonPropertyName("collections")]
    public List<CollectionDto> Dtos { get; set; } = new();

    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = new();
}
