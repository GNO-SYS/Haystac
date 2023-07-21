namespace Haystac.Domain.Events;

public class ItemAddedEvent : BaseEvent
{
    public Item Item { get; }

    public string CollectionId => Item.CollectionIdentifier;

    public ItemAddedEvent(Item item)
    {
        Item = item;
    }
}