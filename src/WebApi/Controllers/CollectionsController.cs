using Microsoft.AspNetCore.Authorization;

using Haystac.Application.Collections.Commands;
using Haystac.Application.Collections.Queries;

using Haystac.Application.Items.Commands;
using Haystac.Application.Items.Queries;

namespace Haystac.WebApi.Controllers;

[Route("collections")]
[ApiController]
public class CollectionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<CollectionListDto>> GetCollections()
        => await _mediator.Send(new GetAllCollectionsQuery());

    [HttpGet("{id}")]
    public async Task<ActionResult<CollectionDto>> GetCollectionById(string id)
        => await _mediator.Send(new GetCollectionByIdentifierQuery { CollectionId = id });

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<Guid>> CreateCollection(
        [FromBody] CollectionDto dto,
        string id,
        [FromQuery] bool anonymous = false)
    {
        if (dto.Identifier != id) return BadRequest($"Payload CollectionID doesn't match route CollectionID");

        return await _mediator.Send(new CreateCollectionCommand { Dto = dto, Anonymous = anonymous });
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateCollection([FromBody] CollectionDto dto, string id)
    {
        if (dto.Identifier != id) return BadRequest($"Payload CollectionID doesn't match route CollectionID");
        
        await _mediator.Send(new UpdateCollectionCommand { CollectionId = id, Dto = dto });

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteCollection(string id)
    {
        await _mediator.Send(new DeleteCollectionCommand { CollectionId = id });

        return NoContent();
    }

    [HttpGet("{id}/items")]
    public async Task<ActionResult<ItemCollectionDto>> GetItemsForCollection(string id)
        => await _mediator.Send(new GetItemsByCollectionQuery { CollectionId = id });

    [HttpGet("{id}/items/{itemId}")]
    public async Task<ActionResult<ItemDto>> GetItemsForCollection(string id, string itemId)
        => await _mediator.Send(new GetItemByIdentifierQuery { Collection = id, Identifier = itemId });

    [Authorize]
    [HttpPost("{id}/items/{itemId}")]
    public async Task<ActionResult<Guid>> CreateItem([FromBody] ItemDto dto, string id, string itemId)
    {
        if (dto.Collection != id) return BadRequest($"Payload CollectionID doesn't match route CollectionID");
        if (dto.Identifier != itemId) return BadRequest($"Payload ItemID doesn't match route ItemID");

        return await _mediator.Send(new CreateItemCommand { Dto = dto });
    }

    [Authorize]
    [HttpPut("{id}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> UpdateItem([FromBody] ItemDto dto, string id, string itemId)
    {
        if (dto.Collection != id) return BadRequest($"Payload CollectionID doesn't match route CollectionID");
        if (dto.Identifier != itemId) return BadRequest($"Payload ItemID doesn't match route ItemID");

        await _mediator.Send(new UpdateItemCommand { Dto = dto });

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> DeleteItem(string id, string itemId)
    {
        await _mediator.Send(new DeleteItemCommand { CollectionId = id, Identifier = itemId });

        return NoContent();
    }
}
