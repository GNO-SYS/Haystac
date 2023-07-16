using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Converters;

namespace Haystac.Domain.Entities;

//< See: https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md
[Table("Items")]
public class Item : BaseStacEntity
{
    public const string DateTimeField = "datetime";

    //< TODO - Technically the spec allows for non-polygonal geometry types in Items, do we care & want to support those?
    //<      - Would suggest that instead we store a 'jsonb' column of whatever geometry we want, as well as a 2D geographic polygon bounding box
    //<      - The 2D polygonal geometry would get ignored when returned to the end user, but would be used for spatial queries
    /// <summary>
    /// Polygonal geometry describing the boundaries of the <see cref="Item"/> entity
    /// </summary>
    [Column(TypeName = "geography (polygon)")]
    [JsonConverter(typeof(GeoJsonConverterFactory))]
    public Polygon Geometry { get; set; } = null!;

    /// <summary>
    /// Key-value collection of additional metadata - only required field is 'datetime'
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Properties { get; set; } = new();

    /// <summary>
    /// List of <see cref="Link"/> objects to resources and related URLs
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<Link> Links { get; set; } = new();

    /// <summary>
    /// A dictionary of additional metadata for the <see cref="Item"/>
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, Asset> Assets { get; set; } = new();

    /// <summary>
    /// The 'id' of the <see cref="Entities.Collection"/> this <see cref="Item"/> belongs to, if allowed
    /// </summary>
    public string CollectionId { get; set; } = string.Empty;

    /// <summary>
    /// The <see cref="Entities.Collection"/> entity that this <see cref="Item"/> belongs to
    /// </summary>
    public Collection Collection { get; set; } = null!;
}
