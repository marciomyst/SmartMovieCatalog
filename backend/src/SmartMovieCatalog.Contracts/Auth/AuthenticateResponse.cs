namespace SmartMovieCatalog.Contracts.Auth;

public sealed record AuthenticateResponse(
    Guid UserId,
    string Email,
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAtUtc);
