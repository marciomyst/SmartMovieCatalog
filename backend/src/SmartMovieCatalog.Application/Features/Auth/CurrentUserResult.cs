namespace SmartMovieCatalog.Application.Features.Auth;

public sealed record CurrentUserResult
{
    private CurrentUserResult(
        bool succeeded,
        AuthenticationFailure? failure,
        Guid userId,
        string? email,
        string? name,
        IReadOnlyCollection<string>? roles,
        bool mustChangePasswordOnFirstLogin)
    {
        Succeeded = succeeded;
        Failure = failure;
        UserId = userId;
        Email = email;
        Name = name;
        Roles = roles ?? Array.Empty<string>();
        MustChangePasswordOnFirstLogin = mustChangePasswordOnFirstLogin;
    }

    public bool Succeeded { get; }

    public AuthenticationFailure? Failure { get; }

    public Guid UserId { get; }

    public string? Email { get; }

    public string? Name { get; }

    public IReadOnlyCollection<string> Roles { get; }

    public bool MustChangePasswordOnFirstLogin { get; }

    public static CurrentUserResult Success(
        Guid userId,
        string email,
        string name,
        IReadOnlyCollection<string> roles,
        bool mustChangePasswordOnFirstLogin)
    {
        return new CurrentUserResult(true, null, userId, email, name, roles, mustChangePasswordOnFirstLogin);
    }

    public static CurrentUserResult Failed(AuthenticationFailure failure)
    {
        return new CurrentUserResult(false, failure, Guid.Empty, null, null, null, false);
    }
}
