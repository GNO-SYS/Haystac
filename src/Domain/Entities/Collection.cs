namespace Haystac.Domain.Entities;

public class Collection : BaseEntity
{

    [JsonIgnore]
    public ICollection<Item> Items { get; set; } = null!;
}
