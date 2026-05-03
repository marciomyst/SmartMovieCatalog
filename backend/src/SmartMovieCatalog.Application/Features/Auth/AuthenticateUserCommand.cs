namespace SmartMovieCatalog.Application.Features.Auth;

public sealed record AuthenticateUserCommand(string? Email, string? Password);
