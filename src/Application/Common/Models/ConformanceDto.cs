namespace Haystac.Application.Common.Models;

public class ConformanceDto
{
    [JsonPropertyName("conformsTo")]
    public List<string> Conformance { get; set; } = new();
}
