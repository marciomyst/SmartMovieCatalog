using FluentValidation.TestHelper;
using SmartMovieCatalog.Api.Features.Movies.CreateMovie;
using SmartMovieCatalog.Contracts.Movies;

namespace SmartMovieCatalog.Api.Tests.Movies;

public sealed class CreateMovieRequestValidatorTests
{
    private readonly CreateMovieRequestValidator _validator = new();

    [Fact]
    public async Task Validate_WithValidRequiredFields_Passes()
    {
        CreateMovieRequest request = new()
        {
            Title = "Central do Brasil",
            ReleaseYear = 1998,
            CountryCode = "BR",
            OriginalLanguage = "pt-BR"
        };

        TestValidationResult<CreateMovieRequest> result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithValidExternalIdAndImage_Passes()
    {
        CreateMovieRequest request = new()
        {
            Title = "Central do Brasil",
            ReleaseYear = 1998,
            CountryCode = "BR",
            OriginalLanguage = "pt-BR",
            ExternalId = 666,
            Image = "/p/example-card.jpg"
        };

        TestValidationResult<CreateMovieRequest> result = await _validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task Validate_WithInvalidFields_ReturnsValidationErrors()
    {
        CreateMovieRequest request = new()
        {
            Title = " ",
            ReleaseYear = 1887,
            CountryCode = "BRA",
            OriginalLanguage = " ",
            Genres = ["Drama", " "],
            DurationMinutes = 0,
            ExternalId = 0,
            Image = "https://image.tmdb.org/t/p/w500/example.jpg"
        };

        TestValidationResult<CreateMovieRequest> result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(movie => movie.Title);
        result.ShouldHaveValidationErrorFor(movie => movie.ReleaseYear);
        result.ShouldHaveValidationErrorFor(movie => movie.CountryCode);
        result.ShouldHaveValidationErrorFor(movie => movie.OriginalLanguage);
        result.ShouldHaveValidationErrorFor("Genres[1]");
        result.ShouldHaveValidationErrorFor(movie => movie.DurationMinutes);
        result.ShouldHaveValidationErrorFor(movie => movie.ExternalId);
        result.ShouldHaveValidationErrorFor(movie => movie.Image);
    }

    [Fact]
    public async Task Validate_WithReleaseYearAfterNextCalendarYear_ReturnsValidationError()
    {
        CreateMovieRequest request = new()
        {
            Title = "Future Movie",
            ReleaseYear = DateTimeOffset.UtcNow.Year + 2,
            CountryCode = "US",
            OriginalLanguage = "en"
        };

        TestValidationResult<CreateMovieRequest> result = await _validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(movie => movie.ReleaseYear);
    }
}
