namespace Haystac.Application.Collections.Queries;

public record GetCollectionsWithPaginationQuery
    : IRequest<PaginatedList<CollectionDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCollectionsWithPaginationQueryHandler 
    : IRequestHandler<GetCollectionsWithPaginationQuery, PaginatedList<CollectionDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCollectionsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<CollectionDto>> Handle(GetCollectionsWithPaginationQuery query,
        CancellationToken cancellationToken)
        => await _context.Collections
                         .OrderBy(x => x.Identifier)
                         .Select(c => c.ToDto())
                         .ToPaginatedListAsync(query.PageNumber, query.PageSize);
}