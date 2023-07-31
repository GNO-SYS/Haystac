using FluentValidation;

namespace Haystac.Application.Collections.Commands;

public class CreateCollectionCommandValidator : AbstractValidator<CreateCollectionCommand>
{
    private readonly IApplicationDbContext _context;

    private readonly ICollection<string> _allowedTypes 
        = new HashSet<string> { "Collection", "Catalog" };

    public CreateCollectionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Dto.Type)
            .NotEmpty().WithMessage("Must specify if this is a 'Collection' or 'Catalog'")
            .MustAsync(BeAllowedType).WithMessage("The 'Type' field must be one of 'Collection' or 'Catalog'");

        RuleFor(v => v.Dto.Identifier)
            .NotEmpty().WithMessage("Collection identifier is required")
            .MustAsync(BeUniqueName).WithMessage("The specified Collection identifier already exists");
    }

    public Task<bool> BeAllowedType(string type, CancellationToken cancellationToken)
        =>  Task.FromResult(_allowedTypes.Contains(type));

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        => await _context.Collections.AllAsync(c => c.Identifier != name, cancellationToken);
}
