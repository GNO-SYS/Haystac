using Haystac.Application.Common.Security;
using Haystac.Domain.Constants;

namespace Haystac.Application.Clients.Commands;

[Authorize(Roles = Roles.Administrator)]
[Authorize(Policy = Policies.CanEditClients)]
public record CreateClientCommand : IRequest<Guid>
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("contact_name")]
    public string ContactName { get; set; } = string.Empty;

    [JsonPropertyName("contact_email")]
    public string ContactEmail { get; set; } = string.Empty;
}

public class CreateClientCommandHandler
    : IRequestHandler<CreateClientCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateClientCommand command, CancellationToken cancellationToken)
    {
        var entity = new Client
        {
            Name = command.Name,
            ContactName = command.ContactName,
            ContactEmail = command.ContactEmail
        };

        _context.Clients.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}