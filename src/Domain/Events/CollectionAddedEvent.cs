namespace Haystac.Domain.Events;

public class CollectionAddedEvent : BaseEvent
{
    public Collection Collection { get; }

    public CollectionAddedEvent(Collection collection)    {
        Collection = collection;
    }
}
