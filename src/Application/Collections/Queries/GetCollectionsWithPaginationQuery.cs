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
    private readonly IClientService _clients;
    private readonly ILinkService _links;

    public GetCollectionsWithPaginationQueryHandler(
        IApplicationDbContext context,
        IClientService clients,
        ILinkService links)
    {
        _context = context;
        _clients = clients;
        _links = links;
    }

    public async Task<PaginatedList<CollectionDto>> Handle(GetCollectionsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var filtered = await _clients.FilterCollectionsAsync(collecs);

        var tasks = filtered.Select(MapCollectionDto);
        var dtos = await Task.WhenAll(tasks);

        return await dtos.AsQueryable().ToPaginatedListAsync(query.PageNumber, query.PageSize);
    }

    async Task<CollectionDto> MapCollectionDto(Collection collec)
    {
        var links = await _links.GenerateCollectionLinks(collec);

        return collec.ToDto(links);
    }
}