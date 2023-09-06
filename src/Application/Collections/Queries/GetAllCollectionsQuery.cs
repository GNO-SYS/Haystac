namespace Haystac.Application.Collections.Queries;

public record GetAllCollectionsQuery : IRequest<List<CollectionDto>> { }

public class GetAllCollectionsQueryHandler 
    : IRequestHandler<GetAllCollectionsQuery, List<CollectionDto>> 
{
    private readonly IApplicationDbContext _context;
    private readonly IClientFilter _filter;

    public GetAllCollectionsQueryHandler(
        IApplicationDbContext context,
        IClientFilter filter)
    {
        _context = context;
        _filter = filter;
    }

    public async Task<List<CollectionDto>> Handle(GetAllCollectionsQuery request,
        CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var filtered = await _filter.FilterAsync(collecs);

        return filtered.Select(c => c.ToDto()).ToList();
    }
}