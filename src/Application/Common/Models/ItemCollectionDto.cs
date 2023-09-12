using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Common.Models;

public class ItemCollectionDto
{
    /// <summary>
    /// [REQUIRED] The GeoJSON 'type' field - always 'FeatureCollection'
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; } = "FeatureCollection";

    /// <summary>
    /// [REQUIRED] A (possibly-empty) list of STAC Item entities
    /// </summary>
    [JsonPropertyName("features")]
    public List<ItemDto> Features { get; set; } = new();

    /// <summary>
    /// A collection of Link objects related to this ItemCollection
    /// </summary>
    [JsonPropertyName("links")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Link>? Links { get; set; }

    /// <summary>
    /// The number of items that meet the selection criteria (possibly estimated)
    /// </summary>
    [JsonPropertyName("numberMatched")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumberMatched { get; set; }

    /// <summary>
    /// The total number of items in the 'features' collection
    /// </summary>
    [JsonPropertyName("numberReturned")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? NumberReturned { get; set; }
}
