using Haystac.Application.Search.Queries;

namespace Haystac.WebApi.Controllers;

[Route("search")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly IMediator _mediator;

    public SearchController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ItemCollectionDto>> SearchByQuery([FromQuery] SearchQuery query)
        => await _mediator.Send(query);

    [HttpPost]
    public async Task<ActionResult<ItemCollectionDto>> SearchByBody([FromBody] SearchQuery query)
        => await _mediator.Send(query);
}
