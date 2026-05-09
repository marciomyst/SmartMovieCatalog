using SmartMovieCatalog.Api.Features.Movies.CreateMovie;
using SmartMovieCatalog.Api.Features.Movies.GetMovieById;
using SmartMovieCatalog.Api.Features.Movies.ListMovies;

namespace SmartMovieCatalog.Api.Features.Movies;

public static class MoviesEndpoints
{
    public static void MapMovieEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapListMovies();
        app.MapGetMovieById();
        app.MapCreateMovie();
    }
}
