namespace SmartMovieCatalog.Application.Features.Auth;

public sealed record AuthenticationResult
{
    private AuthenticationResult(
        bool succeeded,
        AuthenticationFailure? failure,
        Guid userId,
        string? email,
        string? accessToken,
        DateTimeOffset? accessTokenExpiresAtUtc)
    {
        Succeeded = succeeded;
        Failure = failure;
        UserId = userId;
        Email = email;
        AccessToken = accessToken;
        AccessTokenExpiresAtUtc = accessTokenExpiresAtUtc;
    }

    public bool Succeeded { get; }

    public AuthenticationFailure? Failure { get; }

    public Guid UserId { get; }

    public string? Email { get; }

    public string? AccessToken { get; }

    public DateTimeOffset? AccessTokenExpiresAtUtc { get; }

    public static AuthenticationResult Success(Guid userId, string email, string accessToken, DateTimeOffset accessTokenExpiresAtUtc)
    {
        return new AuthenticationResult(true, null, userId, email, accessToken, accessTokenExpiresAtUtc);
    }

    public static AuthenticationResult Failed(AuthenticationFailure failure)
    {
        return new AuthenticationResult(false, failure, Guid.Empty, null, null, null);
    }
}
