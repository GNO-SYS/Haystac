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

    public GetCollectionByIdentifierQueryHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<CollectionDto> Handle(GetCollectionByIdentifierQuery query,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == query.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (entity == null || entity.ClientId != clientId)
        {
            throw new NotFoundException(nameof(Collection), query.CollectionId);
        }

        return entity.ToDto();
    }
}