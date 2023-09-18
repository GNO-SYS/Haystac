using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Catalogs.Queries;

public record GetRootCatalogQuery : IRequest<RootCatalogDto> { }

public class GetRootCatalogQueryHandler
    : IRequestHandler<GetRootCatalogQuery, RootCatalogDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IRootCatalogService _root;
    private readonly IUrlService _url;

    public GetRootCatalogQueryHandler(
        IApplicationDbContext context,
        IRootCatalogService root,                                
        IUrlService url)
    {
        _context = context;
        _root = root;
        _url = url;
    }

    public async Task<RootCatalogDto> Handle(GetRootCatalogQuery query, CancellationToken cancellationToken)
    {
        //< Get only the 'anonymous' collections
        var collecs = await _context.Collections
                                    .Where(c => string.IsNullOrEmpty(c.ClientId))
                                    .ToListAsync(cancellationToken);

        var links = await GenerateLinks(collecs);

        var dto = new RootCatalogDto
        {
            StacVersion = _root.StacVersion,
            Identifier = _root.Identifier,
            Title = _root.Title,
            Description = _root.Description,
            Type = "Catalog",
            ConformsTo = new List<string>
            {
                "https://api.stacspec.org/v1.0.0/core",
                "https://api.stacspec.org/v1.0.0/item-search"
            },
            Links = links
        };

        return dto;
    }

    Task<List<Link>> GenerateLinks(IEnumerable<Collection> collecs)
    {
        var baseUrl = _url.GetBaseUrl();

        var links = new List<Link>
        {
            new Link
            {
                Relationship = "root",
                Href = $"{baseUrl}",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "self",
                Href = $"{baseUrl}",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "service-desc",
                Href = $"{baseUrl}/swagger/v1/swagger.json",
                Type = "application/vnd.oai.openapi+json;version=3.0"
            },
            new Link
            {
                Relationship = "search",
                Href = $"{baseUrl}/search",
                Type = "application/geo+json",
                Method = "GET"
            },
            new Link
            {
                Relationship = "search",
                Href = $"{baseUrl}/search",
                Type = "application/geo+json",
                Method = "POST"
            }
        };

        if (collecs.Any())
        {
            var collectionLinks = collecs.Select(Link.GenerateChildLink);
            links.AddRange(collectionLinks);
        }
        
        return Task.FromResult(links);
    }
}