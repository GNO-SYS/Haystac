namespace Haystac.Application.Conformance.Queries;

public record GetConformanceClassesQuery : IRequest<ConformanceDto>
{ }

public class GetConformanceClassesQueryHandler
    : IRequestHandler<GetConformanceClassesQuery, ConformanceDto>
{
    private readonly IConformanceService _conformance;

    public GetConformanceClassesQueryHandler(IConformanceService conformance)
    {
        _conformance = conformance;
    }

    public async Task<ConformanceDto> Handle(GetConformanceClassesQuery request, CancellationToken cancellationToken)
    {
        var conformance = await _conformance.GetConformanceLinksAsync();

        return new ConformanceDto
        {
            Conformance = conformance,
        };
    }
}