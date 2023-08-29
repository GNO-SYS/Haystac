namespace Haystac.Application.Common.Models;

public class RootCatalogDto : CollectionDto
{
    [JsonPropertyName("conformsTo")]
    public List<string> ConformsTo { get; set; } = new();
}