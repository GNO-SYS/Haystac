using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Common.Interfaces;

public interface ILinkService
{
    Task<List<Link>> GenerateRootCatalogLinks(IEnumerable<Collection> collecs);

    Task<List<Link>> GenerateCollectionLinks(Collection collec);

    Task<List<Link>> GenerateCollectionListLinks();

    Task<List<Link>> GenerateItemCollectionLinks(Collection collec);

    Task<List<Link>> GenerateItemLinks(Collection collec, Item item);
}
