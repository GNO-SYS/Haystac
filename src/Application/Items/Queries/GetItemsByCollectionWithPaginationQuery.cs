namespace Haystac.Application.Items.Queries;

public record GetItemsByCollectionWithPaginationQuery 
    : IRequest<PaginatedList<Item>>
{
    public string CollectionId { get; set; } = string.Empty;

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetItemsByCollectionWithPaginationQueryHandler
    : IRequestHandler<GetItemsByCollectionWithPaginationQuery, PaginatedList<Item>>
{
    private readonly IApplicationDbContext _context;

    public GetItemsByCollectionWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Item>> Handle(GetItemsByCollectionWithPaginationQuery query,
        CancellationToken cancellationToken)
        => await _context.Items
                .Where(i => i.CollectionIdentifier == query.CollectionId)
                .OrderBy(i => i.Identifier)
                .ToPaginatedListAsync(query.PageNumber, query.PageSize);
}
