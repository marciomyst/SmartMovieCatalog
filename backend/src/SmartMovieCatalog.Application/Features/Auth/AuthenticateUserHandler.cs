using SmartMovieCatalog.Application;

namespace SmartMovieCatalog.Application.Features.Auth;

public sealed class AuthenticateUserHandler
{
    private readonly AuthenticateUser _authenticateUser;

    public AuthenticateUserHandler(AuthenticateUser authenticateUser)
    {
        _authenticateUser = authenticateUser;
    }

    public Task<Result<AuthenticatedUser, AuthenticationFailure>> Handle(
        AuthenticateUserCommand command,
        CancellationToken cancellationToken)
    {
        return _authenticateUser.AuthenticateAsync(
            command.Email,
            command.Password,
            cancellationToken);
    }
}
