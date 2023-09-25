using Haystac.Application.Catalogs.Queries;
using Haystac.Application.Conformance.Queries;

namespace Haystac.WebApi.Controllers;

[Route("/")]
[ApiController]
public class RootController : ControllerBase
{
    private readonly IMediator _mediator;

    public RootController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<RootCatalogDto>> GetRootCatalog()
        => await _mediator.Send(new GetRootCatalogQuery());

    [HttpGet("conformance")]
    public async Task<ActionResult<ConformanceDto>> GetConformance()
        => await _mediator.Send(new GetConformanceClassesQuery());

    //< TODO - Add the [HttpGet("api")] route & handler
}
