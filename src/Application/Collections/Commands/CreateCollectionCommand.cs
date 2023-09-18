namespace Haystac.Application.Collections.Commands;

public record CreateCollectionCommand : IRequest<Guid>
{
    public CollectionDto Dto { get; set; } = null!;

    public bool Anonymous { get; set; } = false;
}

public class CreateCollectionCommandHandler 
    : IRequestHandler<CreateCollectionCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public CreateCollectionCommandHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<Guid> Handle(CreateCollectionCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Dto.ToCollection();

        if (!command.Anonymous)
        {
            entity.ClientId = await _clients.GetClientIdAsync();
        }

        entity.AddDomainEvent(new CollectionAddedEvent(entity));

        _context.Collections.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
