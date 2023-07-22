namespace Haystac.Application.Collections.Commands;

public record UpdateCollectionCommand : IRequest
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;

    public CollectionDto Dto { get; set; } = null!;
}

public class UpdateCollectionCommandHandler : IRequestHandler<UpdateCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCollectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCollectionCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        entity.StacVersion = command.Dto.StacVersion;
        entity.Extensions = command.Dto.Extensions ?? entity.Extensions;
        entity.Identifier = command.Dto.Identifier;
        entity.Title = command.Dto.Title ?? entity.Title;
        entity.Description = command.Dto.Description;
        entity.Keywords = command.Dto.Keywords ?? entity.Keywords;
        entity.License = command.Dto.License;
        entity.Providers = command.Dto.Providers ?? entity.Providers;
        entity.Extent = command.Dto.Extent;
        entity.Summaries = command.Dto.Summaries ?? entity.Summaries;
        entity.Links = command.Dto.Links;
        entity.Assets = command.Dto.Assets ?? entity.Assets;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
