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

    public GetItemByIdentifierQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ItemDto> Handle(GetItemByIdentifierQuery query,
        CancellationToken cancellationToken)
    {
        var collec = await _context.Collections.Where(c => c.Identifier == query.Collection)
                                               .Include(c => c.Items)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (collec == null) throw new NotFoundException(nameof(Collection), query.Collection);

        var item = collec.Items.FirstOrDefault(i => i.Identifier == query.Identifier);

        if (item == null) throw new NotFoundException(nameof(Item), query.Identifier);

        return item.ToDto();
    }
}
