namespace Haystac.Application.Items.Queries;

public record GetItemByIdentifierQuery : IRequest<ItemDto>
{
    [JsonPropertyName("collection")]
    public string Collection { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Identifier { get; set; } = string.Empty;
}

public class GetItemByIdentifierQueryHandler 
    : IRequestHandler<GetItemByIdentifierQuery, ItemDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;
    private readonly ILinkService _links;

    public GetItemByIdentifierQueryHandler(
        IApplicationDbContext context,
        IClientService clients,
        ILinkService links)
    {
        _context = context;
        _clients = clients;
        _links = links;
    }

    public async Task<ItemDto> Handle(GetItemByIdentifierQuery query,
        CancellationToken cancellationToken)
    {
        var collec = await _context.Collections.Where(c => c.Identifier == query.Collection)
                                               .Include(c => c.Items)
                                               .FirstOrDefaultAsync(cancellationToken);

        var clientId = await _clients.GetClientIdAsync();

        if (collec == null || !await _clients.IsCollectionVisible(collec))
        {
            throw new NotFoundException(nameof(Collection), query.Collection);
        }

        var item = collec.Items.FirstOrDefault(i => i.Identifier == query.Identifier);

        if (item == null) throw new NotFoundException(nameof(Item), query.Identifier);

        var links = await _links.GenerateItemLinks(collec, item);

        return item.ToDto(links);
    }
}
