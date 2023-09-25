using Haystac.Domain.ValueObjects;

using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

namespace Haystac.Application.Common.Models;

public class ItemDto
{
    [JsonPropertyName("type")]
    public string Type { get; } = "Feature";

    [JsonPropertyName("stac_version")]
    public string StacVersion { get; set; } = string.Empty;

    [JsonPropertyName("stac_extensions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Extensions { get; set; }

    [JsonPropertyName("id")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("geometry")]
    [JsonConverter(typeof(GeoJsonConverterFactory))]
    public Polygon Geometry { get; set; } = null!;

    [JsonPropertyName("bbox")]
    public List<double> BoundingBox { get; set; } = new();

    [JsonPropertyName("properties")]
    public Dictionary<string, object> Properties { get; set; } = new();

    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = new();

    [JsonPropertyName("assets")]
    public Dictionary<string, Asset> Assets { get; set; } = new();

    [JsonPropertyName("collection")]
    public string Collection { get; set; } = string.Empty;

    public override string ToString() => JsonSerializer.Serialize(this);
}

public static class ItemDtoExtensions
{
    public static ItemDto ToDto(this Item item, List<Link>? links = null)
    {
        return new ItemDto
        {
            StacVersion = item.StacVersion,
            Extensions = item.Extensions,
            Identifier = item.Identifier,
            Properties = item.Properties,
            Links = links ?? item.Links,
            Assets = item.Assets,
            Collection = item.CollectionIdentifier,
            Geometry = item.Geometry
        };
    }

    public static Item ToItem(this ItemDto dto)
    {
        return new Item
        {
            StacVersion = dto.StacVersion,
            Extensions = dto.Extensions,
            Identifier = dto.Identifier,
            Properties = dto.Properties,
            Links = dto.Links,
            Assets = dto.Assets,
            CollectionIdentifier = dto.Collection,
            Geometry = dto.Geometry
        };
    }
}