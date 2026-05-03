using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Application.Abstractions.Time;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Features.Auth;

public sealed class AuthenticateUser
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IClock _clock;

    public AuthenticateUser(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IAccessTokenService accessTokenService,
        IClock clock)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _accessTokenService = accessTokenService;
        _clock = clock;
    }

    public async Task<AuthenticationResult> AuthenticateAsync(string? email, string? password, CancellationToken cancellationToken)
    {
        if (!EmailAddress.TryCreate(email, out EmailAddress? emailAddress) ||
            emailAddress is null ||
            string.IsNullOrWhiteSpace(password))
        {
            return AuthenticationResult.Failed(AuthenticationFailure.InvalidCredentials);
        }

        User? user = await _userRepository.FindByNormalizedEmailAsync(emailAddress.NormalizedValue, cancellationToken);
        if (user is null || !user.CanAuthenticate)
        {
            return AuthenticationResult.Failed(AuthenticationFailure.InvalidCredentials);
        }

        if (!_passwordHasher.VerifyPassword(user, password))
        {
            return AuthenticationResult.Failed(AuthenticationFailure.InvalidCredentials);
        }

        AccessToken accessToken = _accessTokenService.CreateAccessToken(user, _clock.UtcNow);

        return AuthenticationResult.Success(user.Id, user.Email.Value, accessToken.Value, accessToken.ExpiresAtUtc);
    }
}
