using FluentValidation;

namespace Haystac.Application.Authentication.Commands;

public class PasswordSignInCommandValidator : AbstractValidator<PasswordSignInCommand>
{
    public PasswordSignInCommandValidator()
    {
        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("User name is required.");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
