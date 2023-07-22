namespace Haystac.Application.Items.Commands;

public record UpdateItemCommand : IRequest
{
    public string CollectionId { get; set; } = string.Empty;

    public string Identifier { get; set; } = string.Empty;

    public ItemDto Dto { get; set; } = null!;
}

public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Items.Where(c => c.CollectionIdentifier == command.CollectionId
                                                  && c.Identifier == command.Identifier)
                                         .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Item), command.Identifier);

        entity.StacVersion = command.Dto.StacVersion;
        entity.Extensions = command.Dto.Extensions ?? entity.Extensions;
        entity.Identifier = command.Dto.Identifier;
        entity.Geometry = command.Dto.Geometry;
        entity.Properties = command.Dto.Properties;
        entity.Links = command.Dto.Links;
        entity.Assets = command.Dto.Assets;
        entity.CollectionIdentifier = command.Dto.Collection;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
