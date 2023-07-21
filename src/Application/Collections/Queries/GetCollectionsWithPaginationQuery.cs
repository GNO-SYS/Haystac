namespace Haystac.Application.Collections.Queries;

public record GetCollectionsWithPaginationQuery
    : IRequest<PaginatedList<Collection>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetCollectionsWithPaginationQueryHandler 
    : IRequestHandler<GetCollectionsWithPaginationQuery, PaginatedList<Collection>>
{
    private readonly IApplicationDbContext _context;

    public GetCollectionsWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Collection>> Handle(GetCollectionsWithPaginationQuery query,
        CancellationToken cancellationToken)
        => await _context.Collections
                         .OrderBy(x => x.Identifier)
                         .ToPaginatedListAsync(query.PageNumber, query.PageSize);
}