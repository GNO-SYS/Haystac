namespace Haystac.Application.Items.Commands;

public record CreateItemCommand : IRequest<Guid>
{
    public ItemDto Dto { get; set; } = null!;
}

public class CreateItemCommandHandler 
    : IRequestHandler<CreateItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public CreateItemCommandHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<Guid> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Dto.ToItem();

        var collec = await _context.Collections
            .FirstOrDefaultAsync(c => c.Identifier == entity.CollectionIdentifier, cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (collec == null || !await _clients.IsCollectionVisible(collec))
        {
            throw new NotFoundException(nameof(Collection), entity.CollectionIdentifier);
        }

        entity.CollectionUuid = collec.Id;

        collec.Extent.UpdateToInclude(entity);

        _context.Items.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
