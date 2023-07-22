namespace Haystac.Domain.Common;

public abstract class BaseEntity
{
    //< TODO - Add the 'Identifier' column here, but Items have a composite key
    //<      - that combines their identifier and their collection identifier, remove GUIDs

    /// <summary>
    /// [Primary Key] The UUID associated with the entity
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

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
