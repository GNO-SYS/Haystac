namespace Haystac.Domain.Common;

public abstract class BaseEntity
{
    /// <summary>
    /// [Primary Key] The UUID associated with the entity
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonPropertyName("uuid")]
    public Guid Uuid { get; set; }

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(BaseEvent domainEvent)
        => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}
