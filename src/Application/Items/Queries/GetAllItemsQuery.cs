namespace Haystac.Application.Items.Queries;

public record GetAllItemsQuery : IRequest<List<ItemDto>> { }

public class GetAllItemsQueryHandler 
    : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public GetAllItemsQueryHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<List<ItemDto>> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var filtered = await _clients.FilterCollectionsAsync(collecs);

        var collec_ids = filtered.Select(x => x.Identifier).ToHashSet();

        return await _context.Items
                             .Where(i => collec_ids.Contains(i.CollectionIdentifier))
                             .Select(i => i.ToDto())
                             .ToListAsync(cancellationToken);
    }
}