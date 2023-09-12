namespace Haystac.Application.Items.Commands;

public record DeleteItemCommand : IRequest
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;

    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = string.Empty;
}

public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public DeleteItemCommandHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var collec = await _context.Collections
            .FirstOrDefaultAsync(c => c.Identifier == command.CollectionId, cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (collec == null || !await _clients.IsCollectionVisible(collec))
        {
            throw new NotFoundException(nameof(Collection), command.CollectionId);
        }

        var entity = await _context.Items.Where(c => c.CollectionIdentifier == command.CollectionId 
                                                  && c.Identifier == command.Identifier)
                                         .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Item), command.Identifier);

        _context.Items.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
