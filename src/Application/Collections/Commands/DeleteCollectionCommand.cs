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
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        _context.Collections.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}