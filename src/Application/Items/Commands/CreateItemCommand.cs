namespace Haystac.Application.Items.Commands;

public record CreateItemCommand : IRequest<Guid>
{
    public ItemDto Dto { get; set; } = null!;
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Dto.ToItem();

        var collec = await _context.Collections
            .FirstOrDefaultAsync(c => c.Identifier == entity.CollectionIdentifier, cancellationToken);

        if (collec is null) throw new NotFoundException(nameof(Collection), entity.CollectionIdentifier);

        entity.CollectionUuid = collec.Id;

        entity.AddDomainEvent(new ItemAddedEvent(entity));

        _context.Items.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
