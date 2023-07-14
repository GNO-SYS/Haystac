namespace Haystac.Domain.ValueObjects;

public class Extent : ValueObject
{
    [JsonPropertyName("spatial")]
    SpatialExtent Spatial { get; set; } = new();

    [JsonPropertyName("temporal")]
    TemporalExtent Temporal { get; set; } = new();

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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BoundingBox;
    }
}

public class TemporalExtent : ValueObject
{
    [JsonPropertyName("interval")]
    public List<List<string>> Interval { get; set; } = new();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Interval;
    }
}
