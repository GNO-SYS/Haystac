namespace Haystac.Domain.Entities;

[Table("Clients")]
public class Client : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string ContactName { get; set; } = string.Empty;

    public string ContactEmail { get; set; } = string.Empty;

    public ICollection<Collection> Collections { get; set; } = null!;
}