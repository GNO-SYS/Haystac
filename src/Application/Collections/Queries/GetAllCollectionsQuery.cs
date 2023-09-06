namespace Haystac.Application.Collections.Queries;

public record GetAllCollectionsQuery : IRequest<List<CollectionDto>> { }

public class GetAllCollectionsQueryHandler 
    : IRequestHandler<GetAllCollectionsQuery, List<CollectionDto>> 
{
    private readonly IApplicationDbContext _context;
    private readonly IClientService _clients;

    public GetAllCollectionsQueryHandler(
        IApplicationDbContext context,
        IClientService clients)
    {
        _context = context;
        _clients = clients;
    }

    public async Task<List<CollectionDto>> Handle(GetAllCollectionsQuery request,
        CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var filtered = await _clients.FilterCollectionsAsync(collecs);

        return filtered.Select(c => c.ToDto()).ToList();
    }
}