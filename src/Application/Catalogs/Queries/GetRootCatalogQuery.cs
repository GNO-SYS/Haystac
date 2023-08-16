namespace Haystac.Application.Catalogs.Queries;

public record GetRootCatalogQuery : IRequest<CollectionDto> { }

public class GetRootCatalogQueryHandler
    : IRequestHandler<GetRootCatalogQuery, CollectionDto>
{
    private readonly IApplicationDbContext _context;

    public GetRootCatalogQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CollectionDto> Handle(GetRootCatalogQuery query, CancellationToken cancellationToken)
    {
        var collecs = await _context.Collections.ToListAsync(cancellationToken);

        var dto = new CollectionDto
        {
            Type = "Catalog"
        };

        //< TODO - Populate the Catalog with the above & any routing configuration

        return dto;
    }
}