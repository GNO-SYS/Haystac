using FluentValidation;

namespace Haystac.Application.Collections.Commands;

public class CreateCollectionCommandValidator : AbstractValidator<CreateCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateCollectionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Dto.Identifier)
            .NotEmpty().WithMessage("Collection identifier is required")
            .MustAsync(BeUniqueName).WithMessage("The specified Collection identifier already exists");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        => await _context.Collections.AllAsync(c => c.Identifier != name, cancellationToken);
}
