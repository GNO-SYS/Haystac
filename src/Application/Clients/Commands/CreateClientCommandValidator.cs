using FluentValidation;

namespace Haystac.Application.Clients.Commands;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateClientCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Client 'name' is required")
            .MustAsync(BeUniqueName).WithMessage("The specified Client already exists");
    }

    public async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        => await _context.Clients.AllAsync(c => c.Name != name, cancellationToken);
}
