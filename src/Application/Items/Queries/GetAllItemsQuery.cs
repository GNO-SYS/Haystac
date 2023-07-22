namespace Haystac.Application.Items.Queries;

public record GetAllItemsQuery : IRequest<List<ItemDto>> { }

public class GetAllItemsQueryHandler 
    : IRequestHandler<GetAllItemsQuery, List<ItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ItemDto>> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
        => await _context.Items
                         .Select(i => i.ToDto())
                         .ToListAsync(cancellationToken);
}