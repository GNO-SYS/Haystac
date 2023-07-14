namespace Haystac.Domain.ValueObjects;

//< See: https://github.com/radiantearth/stac-spec/blob/master/collection-spec/collection-spec.md#provider-object
public class Provider : ValueObject
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("roles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Roles { get; set; }

    [JsonPropertyName("url")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Url { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;

        if (Description != null) yield return Description;
        if (Roles != null) yield return Roles;
        if (Url != null) yield return Url;
    }
}
