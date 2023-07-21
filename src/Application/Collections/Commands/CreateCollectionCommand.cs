namespace Haystac.Application.Collections.Commands;

public record CreateCollectionCommand : IRequest<Guid>
{
    public CollectionDto Dto { get; set; } = null!;
}

public class CreateCollectionCommandHandler : IRequestHandler<CreateCollectionCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCollectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCollectionCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Dto.ToCollection();

        entity.AddDomainEvent(new CollectionAddedEvent(entity));

        _context.Collections.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
