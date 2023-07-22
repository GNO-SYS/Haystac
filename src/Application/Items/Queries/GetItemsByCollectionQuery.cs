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
    => await _context.Items
                .Where(i => i.CollectionIdentifier == query.CollectionId)
                .OrderBy(i => i.Identifier)
                .Select(i => i.ToDto())
                .ToListAsync(cancellationToken);
}