using Haystac.Application.Catalogs.Queries;

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
    public async Task<ActionResult<CollectionDto>> GetRootCatalog()
        => await _mediator.Send(new GetRootCatalogQuery());

    //< TODO - Actually complete the 'GetRootCatalogQueryHandler'
    //< TODO - Add the [HttpGet("api")] route & handler
}
