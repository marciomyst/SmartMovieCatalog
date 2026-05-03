namespace SmartMovieCatalog.Contracts.Auth;

public sealed record AuthenticateRequest
{
    public string? Email { get; init; }

    public string? Password { get; init; }
}
