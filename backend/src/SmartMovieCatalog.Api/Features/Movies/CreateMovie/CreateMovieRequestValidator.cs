using FluentValidation;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Features.Movies.CreateMovie;

public sealed class CreateMovieRequestValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieRequestValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty();

        RuleFor(request => request.ReleaseYear)
            .NotNull()
            .InclusiveBetween(1888, DateTimeOffset.UtcNow.Year + 1);

        RuleFor(request => request.CountryCode)
            .NotEmpty()
            .Must(countryCode => countryCode is not null &&
                countryCode.Trim().Length == 2 &&
                countryCode.Trim().All(char.IsLetter))
            .WithMessage("Country code must contain exactly two letters.");

        RuleFor(request => request.OriginalLanguage)
            .NotEmpty();

        RuleForEach(request => request.Genres)
            .Must(genre => !string.IsNullOrWhiteSpace(genre))
            .WithMessage("Genre must not be empty.");

        RuleFor(request => request.DurationMinutes)
            .GreaterThan(0)
            .When(request => request.DurationMinutes.HasValue);
    }
}
