namespace Haystac.Application.Common.Interfaces;

public interface IRootCatalogService
{
    string StacVersion { get; }
    string Identifier { get; }
    string Title { get; }
    string Description { get; }
}
