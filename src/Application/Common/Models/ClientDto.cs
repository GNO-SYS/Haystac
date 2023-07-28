namespace Haystac.Application.Common.Models;

public record ClientDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("contact_name")]
    public string ContactName { get; set; } = string.Empty;

    [JsonPropertyName("contact_email")]
    public string ContactEmail { get; set; } = string.Empty;
}
