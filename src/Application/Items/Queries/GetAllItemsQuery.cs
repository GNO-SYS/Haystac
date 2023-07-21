namespace Haystac.Application.Items.Queries;

public record GetAllItemsQuery : IRequest<List<Item>> { }

public class GetAllItemsQueryHandler 
    : IRequestHandler<GetAllItemsQuery, List<Item>>
{
    private readonly IApplicationDbContext _context;

    public GetAllItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Item>> Handle(GetAllItemsQuery query, CancellationToken cancellationToken)
        => await _context.Items.ToListAsync(cancellationToken);
}