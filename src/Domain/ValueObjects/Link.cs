namespace Haystac.Domain.ValueObjects;

//< See: https://github.com/radiantearth/stac-spec/blob/master/item-spec/item-spec.md#link-object
public class Link : ValueObject
{
    /// <summary>
    /// The actual link in the format of a URL (both relative &amp; absolute links are both allowed).
    /// </summary>
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    /// <summary>
    /// The relationship between the current document and the linked document.
    /// </summary>
    [JsonPropertyName("rel")]
    public string Relationship { get; set; } = string.Empty;

    /// <summary>
    /// [Optional] The <see href="https://github.com/radiantearth/stac-spec/blob/master/catalog-spec/catalog-spec.md#media-types">media type</see> of the referenced entity 
    /// </summary>
    [JsonPropertyName("type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Type { get; set; }

    /// <summary>
    /// Human-readable title to be used in rendered displays of the link
    /// </summary>
    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    /// <summary>
    /// HTTP method described by the endpoint at the given link
    /// </summary>
    [JsonPropertyName("method")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Method { get; set; }

    public static Link GenerateChildLink(Collection collec, string baseUrl)
    {
        return new Link
        {
            Relationship = "child",
            Href = $"{baseUrl}/collections/{collec.Identifier}",
            Type = "application/json"
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Href;
        yield return Relationship;

        if (Type != null) yield return Type;
        if (Title != null) yield return Title;
    }
}
