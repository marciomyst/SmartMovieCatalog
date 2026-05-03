using SmartMovieCatalog.Application;

namespace SmartMovieCatalog.Application.Features.Auth;

public sealed class GetCurrentUserHandler
{
    private readonly GetCurrentUser _getCurrentUser;

    public GetCurrentUserHandler(GetCurrentUser getCurrentUser)
    {
        _getCurrentUser = getCurrentUser;
    }

    public Task<Result<CurrentUserProfile, AuthenticationFailure>> Handle(
        GetCurrentUserQuery query,
        CancellationToken cancellationToken)
    {
        return _getCurrentUser.GetAsync(cancellationToken);
    }
}
