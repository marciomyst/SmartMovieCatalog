namespace SmartMovieCatalog.Contracts.Auth;

public sealed record CurrentUserResponse(
    Guid UserId,
    string Email,
    string Name,
    IReadOnlyCollection<string> Roles,
    bool MustChangePasswordOnFirstLogin);
