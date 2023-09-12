using FluentValidation;

namespace Haystac.Application.Search.Queries;

public class SearchQueryValidator : AbstractValidator<SearchQuery>
{
    public SearchQueryValidator()
    {
        RuleFor(v => v.Intersects)
            .Null().When(v => v.BoundingBox != null)
            .WithMessage("Cannot specify both 'bbox' and 'intersects' - choose only one");

        RuleFor(v => v.BoundingBox)
            .Null().When(v => v.Intersects != null)
            .WithMessage("Cannot specify both 'bbox' and 'intersects' - choose only one");

        RuleFor(v => v.BoundingBox)
            .MustAsync(ValidDimensionality).When(v => v.BoundingBox != null)
            .WithMessage("Bounding box must have a count of 2N where N is the number of dimensions");
    }

    public static Task<bool> ValidDimensionality(List<double>? bbox, CancellationToken cancellationToken)
    {
        if (bbox == null) return Task.FromResult(true);

        return Task.FromResult(bbox.Count > 4 && bbox.Count % 2 == 0);
    }
}
