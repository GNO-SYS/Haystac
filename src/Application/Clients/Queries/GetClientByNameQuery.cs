using Haystac.Application.Common.Security;
using Haystac.Domain.Constants;

namespace Haystac.Application.Clients.Queries;

[Authorize(Roles = Roles.Administrator)]
public record GetClientByNameQuery : IRequest<Client>
{
    public string ClientName { get; set; } = string.Empty;
}

public class GetClientByNameQueryHandler
    : IRequestHandler<GetClientByNameQuery, Client>
{
    private readonly IApplicationDbContext _context;

    public GetClientByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Client> Handle(GetClientByNameQuery query, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.Where(c => c.Name == query.ClientName)
                                           .Include(c => c.Collections)
                                           .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Client), query.ClientName);

        return entity;
    }
}