using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Common.Models;

public class CollectionDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("stac_version")]
    public string StacVersion { get; set; } = string.Empty;

    [JsonPropertyName("stac_extensions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Extensions { get; set; }

    [JsonPropertyName("id")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("keywords")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Keywords { get; set; }

    [JsonPropertyName("license")]
    public string License { get; set; } = string.Empty;

    [JsonPropertyName("providers")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Provider>? Providers { get; set; }

    [JsonPropertyName("extent")]
    public Extent Extent { get; set; } = new();

    [JsonPropertyName("summaries")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Summaries { get; set; }

    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = new();

    [JsonPropertyName("assets")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, Asset>? Assets { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this);
}

public static class CollectionDtoExtensions
{
    public static CollectionDto ToDto(this Collection collection,
        List<Link>? links = null)
    {
        return new CollectionDto
        {
            Type = collection.Type,
            StacVersion = collection.StacVersion,
            Extensions = collection.Extensions,
            Identifier = collection.Identifier,
            Title = collection.Title,
            Description = collection.Description,
            Keywords = collection.Keywords,
            License = collection.License,
            Providers = collection.Providers,
            Extent = collection.Extent,
            Summaries = collection.Summaries,
            Links = links ?? collection.Links,
            Assets = collection.Assets
        };
    }

    public static Collection ToCollection(this CollectionDto dto)
    {
        return new Collection
        {
            Type = dto.Type,
            StacVersion = dto.StacVersion,
            Extensions = dto.Extensions,
            Identifier = dto.Identifier,
            Title = dto.Title,
            Description = dto.Description,
            Keywords = dto.Keywords,
            License = dto.License,
            Providers = dto.Providers,
            Extent = dto.Extent,
            Summaries = dto.Summaries,
            Links = dto.Links,
            Assets = dto.Assets
        };
    }
}