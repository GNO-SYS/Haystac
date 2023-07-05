namespace Haystac.Domain.Entities;

public class Item : BaseEntity
{
    /// <summary>
    /// Type of the GeoJSON object - must always be set to 'Feature'
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; } = "Feature";

    /// <summary>
    /// The STAC version this <see cref="Item"/> implements
    /// </summary>
    [JsonPropertyName("stac_version")]
    public string StacVersion { get; set; } = string.Empty;

    /// <summary>
    /// A list of STAC extensions this <see cref="Item"/> implements
    /// </summary>
    [JsonPropertyName("stac_extensions")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Extensions { get; set; }

    /// <summary>
    /// The provider's identifier for the <see cref="Item"/> - should be unique within the <see cref="Entities.Collection"/> that contains the <see cref="Item"/>
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// List of <see cref="Link"/> objects to resources and related URLs
    /// </summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = new();

    /// <summary>
    /// A dictionary of additional metadata for the <see cref="Item"/>
    /// </summary>
    [JsonPropertyName("assets")]
    public Dictionary<string, Asset> Assets { get; set; } = new();

    /// <summary>
    /// The 'id' of the <see cref="Entities.Collection"/> this <see cref="Item"/> belongs to, if allowed
    /// </summary>
    [JsonPropertyName("collection")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CollectionId { get; set; } = string.Empty;

    [JsonIgnore]
    public Collection Collection { get; set; } = null!;
}
