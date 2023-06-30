namespace Haystac.Domain.Events;

public class ItemRemovedEvent : BaseEvent
{
    public Item Item { get; }

    public ItemRemovedEvent(Item item)
    {
        Item = item;
    }

    //< TODO: Contract corresponding Collection's geometry to include added geometry
}