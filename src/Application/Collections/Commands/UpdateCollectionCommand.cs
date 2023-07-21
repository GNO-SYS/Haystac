using Haystac.Domain.ValueObjects;

namespace Haystac.Application.Collections.Commands;

public record UpdateCollectionCommand : IRequest
{
    [JsonPropertyName("collection")]
    public string Collection { get; set; } = string.Empty;

    [JsonPropertyName("new_collection_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NewIdentifier { get; set; }

    [JsonPropertyName("title")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Description { get; set; }

    [JsonPropertyName("extent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Extent? Extent { get; set; }
}

public class UpdateCollectionCommandHandler : IRequestHandler<UpdateCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCollectionCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateCollectionCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Collections.Where(c => c.Identifier == command.Collection)
                                               .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) throw new NotFoundException(nameof(Collection), command.Collection);

        if (command.NewIdentifier != null) entity.Identifier = command.NewIdentifier;
        if (command.Title != null) entity.Title = command.Title;
        if (command.Description != null) entity.Description = command.Description;
        if (command.Extent != null) entity.Extent = command.Extent;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
