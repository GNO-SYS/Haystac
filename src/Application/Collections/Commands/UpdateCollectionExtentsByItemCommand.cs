namespace Haystac.Application.Collections.Commands;

public record UpdateCollectionExtentsByItemCommand : IRequest
{
    public string CollectionId { get; set; } = string.Empty;

    public Item Item { get; set; } = null!;
}

public class UpdateCollectionExtentsByItemCommandHandler : IRequestHandler<UpdateCollectionExtentsByItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCollectionExtentsByItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCollectionExtentsByItemCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        //< Consider re-working this, simply re-create the bounds by looping through all associated items
        //< Turn it into an ad-hoc command that the user calls when they want, rather than per-Item

        entity.Extent.UpdateToInclude(command.Item);

        await _context.SaveChangesAsync(cancellationToken);
    }
}