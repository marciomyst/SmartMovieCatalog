using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class GetCurrentUserHandlerTests
{
    [Fact]
    public async Task Handle_DispatchesGetCurrentUserUseCase()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        users.Add(user);
        FakeCurrentUserPrincipalAccessor principalAccessor = new();
        principalAccessor.SetUser(user);
        GetCurrentUserHandler handler = new(new GetCurrentUser(principalAccessor, users));

        Result<CurrentUserProfile, AuthenticationFailure> result = await handler.Handle(
            new GetCurrentUserQuery(),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(user.Id, result.Value.UserId);
    }
}
