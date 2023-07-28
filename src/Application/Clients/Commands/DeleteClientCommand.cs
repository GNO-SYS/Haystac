using Haystac.Application.Common.Security;
using Haystac.Domain.Constants;

namespace Haystac.Application.Clients.Commands;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanEditClients)]
public record DeleteClientCommand : IRequest
{
    public string ClientName { get; set; } = string.Empty;
}

public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteClientCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.Where(c => c.Name == command.ClientName)
                                           .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Client), command.ClientName);

        //< TODO - Should we delete all child Collections and Items as well? Probably..
        _context.Clients.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }
}