namespace Haystac.Application.Common.Interfaces;

public interface IClientFilter
{
    Task<IEnumerable<Collection>> FilterAsync(IEnumerable<Collection> collections);
}
