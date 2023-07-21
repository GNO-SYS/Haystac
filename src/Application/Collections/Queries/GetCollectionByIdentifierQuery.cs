namespace Haystac.Application.Collections.Queries;

public record GetCollectionByIdentifierQuery 
    : IRequest<CollectionDto>
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;
}

public class GetCollectionByIdentifierQueryHandler 
    : IRequestHandler<GetCollectionByIdentifierQuery, CollectionDto>
{
    private readonly IApplicationDbContext _context;

    public GetCollectionByIdentifierQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionDto> Handle(GetCollectionByIdentifierQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == query.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), query.CollectionId);

        return entity.ToDto();
    }
}