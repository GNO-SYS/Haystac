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
    private readonly IClientService _clients;
    private readonly ILinkService _links;

    public GetCollectionByIdentifierQueryHandler(
        IApplicationDbContext context,
        IClientService clients,
        ILinkService links)
    {
        _context = context;
        _clients = clients;
        _links = links;
    }

    public async Task<CollectionDto> Handle(GetCollectionByIdentifierQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == query.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (entity == null || !await _clients.IsCollectionVisible(entity))
        {
            throw new NotFoundException(nameof(Collection), query.CollectionId);
        }

        var links = await _links.GenerateCollectionLinks(entity);

        return entity.ToDto(links);
    }
}