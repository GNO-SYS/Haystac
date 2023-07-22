namespace Haystac.Domain.ValueObjects;

public class Extent : ValueObject
{
    [JsonPropertyName("spatial")]
    public SpatialExtent Spatial { get; set; } = new();

    [JsonPropertyName("temporal")]
    public TemporalExtent Temporal { get; set; } = new();

    public void UpdateToInclude(Item item)
    {
        Spatial.UpdateExtents(item);
        Temporal.UpdateExtents(item);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Spatial;
        yield return Temporal;
    }
}

public class SpatialExtent : ValueObject
{
    [JsonPropertyName("bbox")]
    public List<List<double>> BoundingBox { get; set; } = new();

    public void UpdateExtents(Item item)
    {
        //< TODO - Parse spatial extents from the Item.Geometry's bounding box, expand if maxima/minima change
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BoundingBox;
    }
}

public class TemporalExtent : ValueObject
{
    [JsonPropertyName("interval")]
    public List<List<string>> Interval { get; set; } = new();

    public void UpdateExtents(Item item)
    {
        //< TODO - Parse datetime(s) from 'properties' map, update if available
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Interval;
    }
}
