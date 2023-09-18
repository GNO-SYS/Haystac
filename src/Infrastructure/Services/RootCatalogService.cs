using Microsoft.Extensions.Options;

namespace Haystac.Infrastructure.Services;

public class RootCatalogOptions
{
    public const string RootCatalog = "RootCatalog";

    public const string DefaultVersion = "1.0.0";
    public const string DefaultIdentifier = "haystac";
    public const string DefaultTitle = "Root catalog for Haytstac API";
    public const string DefaultDescription = "A descriptive message about the Haystac landing page";

    public string StacVersion { get; set; } = DefaultVersion;
    public string Identifier { get; set; } = DefaultIdentifier;
    public string Title { get; set; } = DefaultTitle;
    public string Description { get; set; } = DefaultDescription;
}

public class RootCatalogService : IRootCatalogService
{
    private readonly RootCatalogOptions _options;

    public RootCatalogService(IOptions<RootCatalogOptions> options)
    {
        _options = options.Value;
    }

    public string StacVersion => _options.StacVersion;
    public string Identifier => _options.Identifier;
    public string Title => _options.Title;
    public string Description => _options.Description;
}
