namespace Haystac.Application.Items.Commands;

public record CreateItemCommand : IRequest<Guid>
{
    public ItemDto Dto { get; set; } = null!;
}

public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var entity = command.Dto.ToItem();

        _context.Items.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
