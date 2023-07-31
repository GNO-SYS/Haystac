namespace Haystac.Domain.Entities;

//< See: https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md
[Table("Collections")]
public class Collection : BaseStacEntity
{
    /// <summary>
    /// The type flag of the <see cref="Collection"/> (or Catalog) entity - only 'Collection' and 'Catalog' are allowed
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// A short, descriptive one-line title for the <see cref="Collection"/> entity
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// A detailed, multi-line description to fully explain the <see cref="Collection"/> entity
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// List of text keywords that help categorize the <see cref="Collection"/> entity
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<string>? Keywords { get; set; }

    /// <summary>
    /// The <see cref="Collection"/> entity's license - must be SPDX (<see href="https://spdx.org/licenses/"/>), 'various', or 'proprietary'
    /// </summary>
    public string License { get; set; } = string.Empty;

    /// <summary>
    /// A list of <see cref="Provider"/> entities - may include organizations capturing & processing the data or the hosting provider.
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<Provider>? Providers { get; set; }

    /// <summary>
    /// The Spatial & Temporal extents of this <see cref="Collection"/> entity
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Extent Extent { get; set; } = new();

    /// <summary>
    /// A map of property summaries - either a set of values, a range of values, or a JSON schema
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, object>? Summaries { get; set; }

    /// <summary>
    /// List of <see cref="Link"/> objects to resources and related URLs
    /// </summary>
    [Column(TypeName = "jsonb")]
    public List<Link> Links { get; set; } = new();

    /// <summary>
    /// A dictionary of additional metadata for the <see cref="Item"/>
    /// </summary>
    [Column(TypeName = "jsonb")]
    public Dictionary<string, Asset>? Assets { get; set; }

    public Client? Client { get; set; }

    /// <summary>
    /// A collection of all <see cref="Item"/> entities contained by this <see cref="Collection"/> entity
    /// </summary>
    public ICollection<Item> Items { get; set; } = null!;
}
