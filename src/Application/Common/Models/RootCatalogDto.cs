using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Common.Models;

public class RootCatalogDto
{
    [JsonPropertyName("stac_version")]
    public string StacVersion { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("conformsTo")]
    public List<string> ConformsTo { get; set; } = new();

    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = new();
}