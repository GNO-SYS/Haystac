using Haystac.Application.Common.Security;
using Haystac.Domain.Constants;

namespace Haystac.Application.Clients.Commands;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanEditClients)]
public record UpdateClientCommand : IRequest
{
    public string ClientName { get; set; } = string.Empty;

    public ClientDto Dto { get; set; } = null!;
}

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateClientCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients.Where(c => c.Name == command.ClientName)
                                           .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Client), command.ClientName);

        entity.Name = command.Dto.Name;
        entity.ContactName = command.Dto.ContactName;
        entity.ContactEmail = command.Dto.ContactEmail;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
