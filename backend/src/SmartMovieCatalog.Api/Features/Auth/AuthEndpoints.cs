using SmartMovieCatalog.Api.Features.Auth.Authenticate;
using SmartMovieCatalog.Api.Features.Auth.GetCurrentUser;

namespace SmartMovieCatalog.Api.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapAuthenticate();
        app.MapGetCurrentUser();
    }
}
