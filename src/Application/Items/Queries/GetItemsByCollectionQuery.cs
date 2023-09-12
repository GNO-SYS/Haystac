namespace Haystac.Application.Items.Queries;

public record GetItemsByCollectionQuery : IRequest<ItemCollectionDto>
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;
}

public class GetItemsByCollectionQueryHandler : IRequestHandler<GetItemsByCollectionQuery, ItemCollectionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public GetItemsByCollectionQueryHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<ItemCollectionDto> Handle(GetItemsByCollectionQuery query, CancellationToken cancellationToken)
    {
        var collec = await _context.Collections.Where(c => c.Identifier == query.CollectionId)
                                               .Include(c => c.Items)
                                               .FirstOrDefaultAsync(cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (collec == null || !await _clients.IsCollectionVisible(collec))
        {
            throw new NotFoundException(nameof(Collection), query.CollectionId);
        }

        var dto = new ItemCollectionDto
        {
            Features = collec.Items.Select(i => i.ToDto()).ToList(),
            NumberMatched = collec.Items.Count,
            NumberReturned = collec.Items.Count
        };

        return dto;
    }
}