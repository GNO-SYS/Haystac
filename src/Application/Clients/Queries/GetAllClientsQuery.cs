namespace Haystac.Application.Clients.Queries;

public record GetAllClientsQuery : IRequest<List<Client>> { }

public class GetAllClientsQueryHandler
    : IRequestHandler<GetAllClientsQuery, List<Client>>
{
    private readonly IApplicationDbContext _context;

    public GetAllClientsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Client>> Handle(GetAllClientsQuery query, CancellationToken cancellationToken)
        => await _context.Clients
                         .Include(c => c.Collections)
                         .ToListAsync(cancellationToken);
}