using FluentValidation;

namespace Haystac.Application.Collections.Queries;

public class GetCollectionsWithPaginationQueryValidator
    : AbstractValidator<GetCollectionsWithPaginationQuery>
{
    public GetCollectionsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}