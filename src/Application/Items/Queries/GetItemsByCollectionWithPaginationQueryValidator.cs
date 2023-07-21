using FluentValidation;

namespace Haystac.Application.Items.Queries;

internal class GetItemsByCollectionWithPaginationQueryValidator
    : AbstractValidator<GetItemsByCollectionWithPaginationQuery>
{
    public GetItemsByCollectionWithPaginationQueryValidator()
    {
        RuleFor(x => x.CollectionId)
            .NotEmpty().WithMessage("CollectionId is required");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}