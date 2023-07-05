namespace Haystac.Domain.ValueObjects;

//< See: https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md#asset-object
public class Asset : ValueObject
{
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; } 

    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    [JsonPropertyName("roles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Roles { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Href;

        if (Title != null) yield return Title;
        if (Description != null) yield return Description;
        if (Type != null) yield return Type;
        if (Roles != null) yield return Roles;
    }
}