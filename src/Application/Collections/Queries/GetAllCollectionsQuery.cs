namespace Haystac.Application.Collections.Queries;

public record GetAllCollectionsQuery : IRequest<List<CollectionDto>> { }

public class GetAllCollectionsQueryHandler 
    : IRequestHandler<GetAllCollectionsQuery, List<CollectionDto>> 
{
    private readonly IApplicationDbContext _context;

    public GetAllCollectionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CollectionDto>> Handle(GetAllCollectionsQuery request, CancellationToken cancellationToken)
        => await _context.Collections
                         .Select(c => c.ToDto())
                         .ToListAsync(cancellationToken);
}