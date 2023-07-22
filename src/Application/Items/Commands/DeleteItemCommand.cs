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

    public DeleteItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteItemCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Items.Where(c => c.CollectionIdentifier == command.CollectionId 
                                                  && c.Identifier == command.Identifier)
                                         .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Item), command.Identifier);

        _context.Items.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
