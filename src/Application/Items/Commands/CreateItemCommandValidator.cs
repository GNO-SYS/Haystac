using FluentValidation;

namespace Haystac.Application.Items.Commands;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateItemCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Dto.Collection)
            .NotEmpty().WithMessage("Collection identifier is required")
            .MustAsync(CollectionExists).WithMessage("The specified Collection name does not exist");
    }

    public async Task<bool> CollectionExists(string collectionId, CancellationToken cancellationToken)
        => await _context.Collections.AnyAsync(c => c.Identifier == collectionId, cancellationToken);
}
