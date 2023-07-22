namespace Haystac.Application.Items.Queries;

public record GetItemsByCollectionQuery : IRequest<List<ItemDto>>
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;
}

public class GetItemsByCollectionQueryHandler : IRequestHandler<GetItemsByCollectionQuery, List<ItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetItemsByCollectionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ItemDto>> Handle(GetItemsByCollectionQuery query, CancellationToken cancellationToken)
    {
        var collec = await _context.Collections.Where(c => c.Identifier == query.CollectionId)
                                               .Include(c => c.Items)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (collec == null) throw new NotFoundException(nameof(Collection), query.CollectionId);

        return collec.Items.Select(i => i.ToDto()).ToList();
    }
}