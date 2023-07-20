namespace Haystac.Application.Collections.Queries;

public record GetCollectionByNameCommand : IRequest<CollectionDto>
{
    [JsonPropertyName("collection")]
    public string CollectionId { get; set; } = string.Empty;
}

public class GetCollectionByNameCommandHandler 
    : IRequestHandler<GetCollectionByNameCommand, CollectionDto>
{
    private readonly IApplicationDbContext _context;

    public GetCollectionByNameCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionDto> Handle(GetCollectionByNameCommand command,
        CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.CollectionId)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.CollectionId);

        return entity.ToDto();
    }
}