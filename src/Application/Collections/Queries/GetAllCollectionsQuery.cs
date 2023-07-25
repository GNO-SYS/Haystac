namespace Haystac.Application.Collections.Queries;

public record GetAllCollectionsQuery : IRequest<List<Collection>> { }

public class GetAllCollectionsQueryHandler 
    : IRequestHandler<GetAllCollectionsQuery, List<Collection>> 
{
    private readonly IApplicationDbContext _context;

    public GetAllCollectionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Collection>> Handle(GetAllCollectionsQuery request, CancellationToken cancellationToken)
        => await _context.Collections
                         .Include(c => c.Items)
                         .ToListAsync(cancellationToken);
}