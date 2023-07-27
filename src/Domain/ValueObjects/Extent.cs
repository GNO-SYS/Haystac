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
        var bounds = item.Geometry.EnvelopeInternal;
        var currBounds = BoundingBox.First();
        BoundingBox[0] = new List<double>
        {
            Math.Min(currBounds[0], bounds.MinX),
            Math.Min(currBounds[1], bounds.MinY),
            Math.Max(currBounds[2], bounds.MaxX),
            Math.Max(currBounds[3], bounds.MaxY)
        };
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
