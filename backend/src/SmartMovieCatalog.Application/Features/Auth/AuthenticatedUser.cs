namespace SmartMovieCatalog.Application.Features.Auth;

public sealed record AuthenticatedUser(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc);
