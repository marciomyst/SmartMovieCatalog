namespace SmartMovieCatalog.Application.Features.Auth;

public sealed record CurrentUserProfile(
    Guid UserId,
    string Email,
    string Name,
    IReadOnlyCollection<string> Roles,
    bool MustChangePasswordOnFirstLogin);
