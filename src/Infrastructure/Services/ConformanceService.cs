namespace Haystac.Infrastructure.Services;

public class ConformanceService : IConformanceService
{
    public Task<List<string>> GetConformanceLinksAsync()
    {
        var links = new List<string>
        {
            "https://api.stacspec.org/v1.0.0-beta.5/core",
            "https://api.stacspec.org/v1.0.0-beta.5/collections",
            "https://api.stacspec.org/v1.0.0-beta.5/ogcapi-features",
            //"https://api.stacspec.org/v1.0.0-beta.5/item-search",
            "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/core",
            "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/oas30",
            "http://www.opengis.net/spec/ogcapi-features-1/1.0/conf/geojson"
        };

        return Task.FromResult(links);
    }
}
