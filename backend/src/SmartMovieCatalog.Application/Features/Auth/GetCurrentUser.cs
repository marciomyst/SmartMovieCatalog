using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Features.Auth;

public sealed class GetCurrentUser
{
    private readonly ICurrentUserPrincipalAccessor _principalAccessor;
    private readonly IUserRepository _userRepository;

    public GetCurrentUser(
        ICurrentUserPrincipalAccessor principalAccessor,
        IUserRepository userRepository)
    {
        _principalAccessor = principalAccessor;
        _userRepository = userRepository;
    }

    public async Task<CurrentUserResult> GetAsync(CancellationToken cancellationToken)
    {
        CurrentUserPrincipal? principal = _principalAccessor.GetCurrentUserPrincipal();
        if (principal is null)
        {
            return CurrentUserResult.Failed(AuthenticationFailure.Unauthenticated);
        }

        User? user = await _userRepository.GetByIdAsync(principal.UserId, cancellationToken);
        if (user is null || !user.CanAuthenticate)
        {
            return CurrentUserResult.Failed(AuthenticationFailure.UserUnavailable);
        }

        string[] roles = user.Roles
            .Select(role => role.Name)
            .ToArray();

        return CurrentUserResult.Success(
            user.Id,
            user.Email.Value,
            user.Name,
            roles,
            user.MustChangePasswordOnFirstLogin);
    }
}
