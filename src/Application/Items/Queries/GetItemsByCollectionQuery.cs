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
    private readonly ILinkService _links;

    public GetItemsByCollectionQueryHandler(
        IApplicationDbContext context,
        IClientService clients,
        ILinkService links)
    {
        _context = context;
        _clients = clients;
        _links = links;
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

        var links = await _links.GenerateItemCollectionLinks(collec);

        var tasks = collec.Items.Select(i => MapItemDto(collec, i));
        var dtos = await Task.WhenAll(tasks);

        var dto = new ItemCollectionDto
        {
            Features = dtos.ToList(),
            NumberMatched = collec.Items.Count,
            NumberReturned = collec.Items.Count,
            Links = links
        };

        return dto;
    }

    async Task<ItemDto> MapItemDto(Collection collec, Item item)
    {
        var links = await _links.GenerateItemLinks(collec, item);

        return item.ToDto(links);
    }
}