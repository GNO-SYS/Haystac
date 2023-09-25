namespace Haystac.Application.Catalogs.Queries;

public record GetRootCatalogQuery : IRequest<RootCatalogDto> { }

public class GetRootCatalogQueryHandler
    : IRequestHandler<GetRootCatalogQuery, RootCatalogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IConformanceService _conformance;
    private readonly IRootCatalogService _root;
    private readonly ILinkService _links;

    public GetRootCatalogQueryHandler(
        IApplicationDbContext context,
        IConformanceService conformance,
        IRootCatalogService root,
        ILinkService links)
    {
        _context = context;
        _conformance = conformance;
        _root = root;
        _links = links;
    }

    public async Task<RootCatalogDto> Handle(GetRootCatalogQuery query, CancellationToken cancellationToken)
    {
        //< Get only the 'anonymous' collections
        var collecs = await _context.Collections
                                    .Where(c => string.IsNullOrEmpty(c.ClientId))
                                    .ToListAsync(cancellationToken);

        var links = await _links.GenerateRootCatalogLinks(collecs);
        var conformance = await _conformance.GetConformanceLinksAsync();

        return new RootCatalogDto
        {
            StacVersion = _root.StacVersion,
            Identifier = _root.Identifier,
            Title = _root.Title,
            Description = _root.Description,
            Type = "Catalog",
            ConformsTo = conformance,
            Links = links
        };
    }
}