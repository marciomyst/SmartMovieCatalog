using SmartMovieCatalog.Api.Features.Movies.CreateMovie;

namespace SmartMovieCatalog.Api.Features.Movies;

public static class MoviesEndpoints
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapCreateMovie();
    }
}
