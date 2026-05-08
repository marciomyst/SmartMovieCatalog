using FluentValidation;

namespace SmartMovieCatalog.Api.Features.Movies.ListMovies;

public sealed class ListMoviesRequestValidator : AbstractValidator<ListMoviesRequest>
{
    public ListMoviesRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(request => request.PageSize)
            .GreaterThanOrEqualTo(1);
    }
}
