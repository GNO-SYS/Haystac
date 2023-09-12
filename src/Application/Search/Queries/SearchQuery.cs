using NetTopologySuite.Geometries;

namespace Haystac.Application.Search.Queries;

public record SearchQuery : IRequest<ItemCollectionDto>
{
    /// <summary>
    /// The maximum number of results to return (page size)
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; } = 100;

    /// <summary>
    /// The requested bounding box, where the length is 2N (N being dimensions)
    /// <para>NOTE: Order values as all axes of SW corner first, then all axes of NE corner</para>
    /// </summary>
    [JsonPropertyName("bbox")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<double>? BoundingBox { get; set; }

    /// <summary>
    /// Single datetime (or range with '/' separator), '..' is allowed for open date ranges
    /// </summary>
    [JsonPropertyName("datetime")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DateTime { get; set; }

    /// <summary>
    /// [UNSUPPORTED] The generic GeoJSON geometry that all resultant items must intersect with
    /// </summary>
    [JsonPropertyName("intersects")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Intersects { get; set; }

    /// <summary>
    /// A collection of item IDs to return
    /// </summary>
    [JsonPropertyName("ids")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? ItemIds { get; set; }

    /// <summary>
    /// A collection of one or more Collection IDs that each matching item must be a part of
    /// </summary>
    [JsonPropertyName("collections")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? CollectionIds { get; set; }

    public Polygon? GetBoundingPolygon()
    {
        if (BoundingBox == null) return null;

        var coords = GetCoordinates(BoundingBox);

        return new Polygon(new LinearRing(coords));
    }

    static Coordinate[] GetCoordinates(List<double> bbox)
    {
        bool is3D = bbox.Count > 4;

        double minLon = bbox[0];
        double minLat = bbox[1];
        double maxLon = is3D ? bbox[3] : bbox[2];
        double maxLat = is3D ? bbox[4] : bbox[3];

        Coordinate LL = new(minLon, minLat);
        Coordinate UL = new(minLon, maxLat);
        Coordinate UR = new(maxLon, maxLat);
        Coordinate LR = new(maxLon, minLat);

        return new Coordinate[]
        {
            LL, UL, UR, LR, LL
        };
    }
}

public class SearchQueryHandler : IRequestHandler<SearchQuery, ItemCollectionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public SearchQueryHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<ItemCollectionDto> Handle(SearchQuery query, CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.OrderBy(c => c.Identifier)
                                                .Include(c => c.Items)
                                                .ToListAsync(cancellationToken);

        var filtered =  await _clients.FilterCollectionsAsync(collecs);

        var items = await GetMatchingItemsAsync(filtered, query);

        //< TODO - Add actual paging - the STAC paging mechanism.. hurts

        var res = new ItemCollectionDto
        {
            Features = items.Select(i => i.ToDto()).ToList(),
            NumberMatched = items.Count(),
            NumberReturned = items.Count()
        };

        return res;
    }

    static async Task<IEnumerable<Item>> GetMatchingItemsAsync(IEnumerable<Collection> collecs, SearchQuery query)
    {
        var tasks = collecs.Select(c => FilterItemsAsync(c, query));

        var items = await Task.WhenAll(tasks);

        return items.Where(i => i.Any())
                    .SelectMany(i => i);
    }

    static Task<List<Item>> FilterItemsAsync(Collection collec, SearchQuery query)
    {
        if (query.CollectionIds != null && !query.CollectionIds.Contains(collec.Identifier))
        {
            return Task.FromResult(new List<Item>());
        }

        var poly = query.GetBoundingPolygon();
        var items = new List<Item>();

        foreach (var item in collec.Items)
        {
            if (poly != null && !poly.Intersects(item.Geometry))
            {
                continue;
            }

            if (query.ItemIds != null && !query.ItemIds.Contains(item.Identifier))
            {
                continue;
            }

            //< TODO - Add temporal search

            items.Add(item);
        }

        return Task.FromResult(items);
    }
}