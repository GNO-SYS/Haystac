using Haystac.Domain.ValueObjects;

namespace Haystac.Infrastructure.Services;

public class LinkService : ILinkService
{
    private readonly IUrlService _url;

    public LinkService(IUrlService url)
    {
        _url = url;
    }

    public Task<List<Link>> GenerateCollectionLinks(Collection collec)
    {
        var baseUrl = _url.GetBaseUrl();

        var links = new List<Link>
        {
            new Link
            {
                Relationship = "root",
                Href = $"{baseUrl}/",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "parent",
                Href = $"{baseUrl}/",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "self",
                Href = $"{baseUrl}/collections/{collec.Identifier}",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "items",
                Href = $"{baseUrl}/collections/{collec.Identifier}/items",
                Type = "application/geo+json"
            },
            //< TODO - Consider adding 'Parent' link if viable
        };

        return Task.FromResult(links);
    }

    public Task<List<Link>> GenerateCollectionListLinks()
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
                Href = $"{baseUrl}/collections",
                Type = "application/json"
            }
        };

        return Task.FromResult(links);
    }

    public Task<List<Link>> GenerateItemCollectionLinks(Collection collec)
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
                Href = $"{baseUrl}/collections/{collec.Identifier}/items",
                Type = "application/geo+json"
            },
            new Link
            {
                Relationship = "collection",
                Href = $"{baseUrl}/collections/{collec.Identifier}",
                Type = "application/json"
            }
        };

        return Task.FromResult(links);
    }

    public Task<List<Link>> GenerateItemLinks(Collection collec, Item item)
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
                Href = $"{baseUrl}/collections/{collec.Identifier}/items/{item.Identifier}",
                Type = "application/geo+json"
            },
            new Link
            {
                Relationship = "collection",
                Href = $"{baseUrl}/collections/{collec.Identifier}",
                Type = "application/json"
            }
        };

        return Task.FromResult(links);
    }

    public Task<List<Link>> GenerateRootCatalogLinks(IEnumerable<Collection> collecs)
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
                Relationship = "data",
                Href = $"{baseUrl}/collections",
                Type = "application/json"
            },
            new Link
            {
                Relationship = "conformance",
                Href = $"{baseUrl}/conformance",
                Type = "application/json"
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
            var collectionLinks = collecs.Select(c => Link.GenerateChildLink(c, baseUrl));
            links.AddRange(collectionLinks);
        }

        return Task.FromResult(links);
    }
}
