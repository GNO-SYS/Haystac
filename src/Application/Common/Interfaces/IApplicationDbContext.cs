namespace Haystac.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Item> Items { get; }
    DbSet<Collection> Collections { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}