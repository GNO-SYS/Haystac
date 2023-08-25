using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Catalogs.Queries;

public record GetRootCatalogQuery : IRequest<CollectionDto> { }

public class GetRootCatalogQueryHandler
    : IRequestHandler<GetRootCatalogQuery, CollectionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IUrlService _url;

    public GetRootCatalogQueryHandler(IApplicationDbContext context,
                                      IUrlService url)
    {
        _context = context;
        _url = url;
    }

    public async Task<CollectionDto> Handle(GetRootCatalogQuery query, CancellationToken cancellationToken)
    {
        //< Get only the 'anonymous' collections
        var collecs = await _context.Collections
                                    .Where(c => string.IsNullOrEmpty(c.ClientId))
                                    .ToListAsync(cancellationToken);

        var links = await GenerateLinks(collecs);

        var dto = new CollectionDto
        {
            StacVersion = "1.0.0",
            Identifier = "haystac-root",
            Title = "Root catalog for Haytstac API",
            Type = "Catalog",
            Links = links
        };

        //< TODO - Handle 'Conforms to' tags - where is that?
        //< TODO - How handle Identifier / Title / Description / version, etc?
        //<         .. We may need to store a 'root' Catalog, and make this one on-demand if needed
        //<      - Leaning on the side of maintaing a 'haystac-root' catalog, that is seeded as above
        //<      - Users can then update it as they see fit, we don't have to manage it all

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
            }
        };

        var collectionLinks = collecs.Select(Link.GenerateChildLink);
        links.AddRange(collectionLinks);

        return Task.FromResult(links);
    }
}