namespace Haystac.Application.Collections.Commands;

public record DeleteCollectionCommand : IRequest
{
    [JsonPropertyName("identifier")]
    public string CollectionId { get; set; } = string.Empty;
}

public class DeleteCollectionCommandHandler : IRequestHandler<DeleteCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCollectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteCollectionCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.CollectionId)
                                               .Include(c => c.Items)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        //< Remove all child Items
        foreach (var item in entity.Items) _context.Items.Remove(item);
        //< Remove the Collection itself
        _context.Collections.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}