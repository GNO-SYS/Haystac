namespace Haystac.Application.Collections.Queries;

public record GetAllCollectionsQuery : IRequest<CollectionListDto> { }

public class GetAllCollectionsQueryHandler 
    : IRequestHandler<GetAllCollectionsQuery, CollectionListDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;
    private readonly ILinkService _links;

    public GetAllCollectionsQueryHandler(
        IApplicationDbContext context,
        IClientService clients,
        ILinkService links)
    {
        _context = context;
        _clients = clients;
        _links = links;
    }

    public async Task<CollectionListDto> Handle(GetAllCollectionsQuery request,
        CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var filtered = await _clients.FilterCollectionsAsync(collecs);

        var tasks = filtered.Select(MapCollectionDto);
        var dtos = await Task.WhenAll(tasks);

        var dto = new CollectionListDto
        {
            Dtos = dtos.ToList(),
            Links = await _links.GenerateCollectionListLinks()
        };

        return dto;
    }

    async Task<CollectionDto> MapCollectionDto(Collection collec)
    {
        var links = await _links.GenerateCollectionLinks(collec);

        return collec.ToDto(links);
    }
}