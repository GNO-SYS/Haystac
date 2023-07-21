namespace Haystac.Application.Collections.Queries;

public record GetCollectionByIdentifierCommand : IRequest<CollectionDto>
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;
}

public class GetCollectionByIdentifierCommandHandler 
    : IRequestHandler<GetCollectionByIdentifierCommand, CollectionDto>
{
    private readonly IApplicationDbContext _context;

    public GetCollectionByIdentifierCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionDto> Handle(GetCollectionByIdentifierCommand command,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        return entity.ToDto();
    }
}