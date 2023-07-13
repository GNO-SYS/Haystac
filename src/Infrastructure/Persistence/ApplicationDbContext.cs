using MediatR;

using Haystac.Application.Common.Interfaces;

namespace Haystac.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Client> Clients => Set<Client>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //builder.HasPostgresExtension("postgis"); TODO - Enable this once NPGSQL is added

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
