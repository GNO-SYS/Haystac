using Haystac.Application.Common.Exceptions;

namespace Haystac.Application.Items.Queries;

public record GetItemByIdentifierQuery : IRequest<ItemDto>
{
    [JsonPropertyName("collection")]
    public string Collection { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Identifier { get; set; } = string.Empty;
}

public class GetItemByIdentifierQueryHandler : IRequestHandler<GetItemByIdentifierQuery, ItemDto>
{
    private readonly IApplicationDbContext _context;

    public GetItemByIdentifierQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ItemDto> Handle(GetItemByIdentifierQuery query, CancellationToken cancellationToken)
    {
        var entity = await _context.Items.Where(i => i.Identifier == query.Identifier && i.CollectionId == query.Collection)
                                         .FirstOrDefaultAsync();

        if (entity == null) throw new NotFoundException(nameof(Item), query.Identifier);

        return entity.ToDto();
    }
}
