namespace Haystac.Application.Items.Queries;

public record GetItemsByCollectionWithPaginationQuery 
    : IRequest<PaginatedList<ItemDto>>
{
    public string CollectionId { get; set; } = string.Empty;

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetItemsByCollectionWithPaginationQueryHandler
    : IRequestHandler<GetItemsByCollectionWithPaginationQuery, PaginatedList<ItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetItemsByCollectionWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<ItemDto>> Handle(GetItemsByCollectionWithPaginationQuery query,
        CancellationToken cancellationToken)
        => await _context.Items
                .Where(i => i.CollectionIdentifier == query.CollectionId)
                .OrderBy(i => i.Identifier)
                .Select(i => i.ToDto())
                .ToPaginatedListAsync(query.PageNumber, query.PageSize);
}
