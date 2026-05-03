using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Tests.TestSupport;

public sealed class FakeCurrentUserPrincipalAccessor : ICurrentUserPrincipalAccessor
{
    public CurrentUserPrincipal? Principal { get; set; }

    public CurrentUserPrincipal? GetCurrentUserPrincipal()
    {
        return Principal;
    }

    public void SetUser(User user)
    {
        Principal = new CurrentUserPrincipal(UserId.From(user.Id));
    }
}
