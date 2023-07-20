using Haystac.Application.Collections.Commands;

namespace Haystac.Application.EventHandlers;

public class ItemAddedEventHandler
    : INotificationHandler<ItemAddedEvent>
{
    private readonly ILogger<ItemAddedEventHandler> _logger;
    private readonly IMediator _mediator;

    public ItemAddedEventHandler(ILogger<ItemAddedEventHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task Handle(ItemAddedEvent itemAddedEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Expanding Collection extents for: {CollectionName}", itemAddedEvent.CollectionId);

        await _mediator.Send(new UpdateCollectionExtentsByItemCommand
        {
            CollectionId = itemAddedEvent.CollectionId,
            Item = itemAddedEvent.Item
        }, cancellationToken);
    }
}
