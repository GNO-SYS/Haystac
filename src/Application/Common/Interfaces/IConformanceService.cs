namespace Haystac.Application.Common.Interfaces;

public interface IConformanceService
{
    Task<List<string>> GetConformanceLinksAsync();
}
