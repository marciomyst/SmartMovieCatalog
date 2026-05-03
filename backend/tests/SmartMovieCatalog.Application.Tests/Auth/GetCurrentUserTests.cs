using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class GetCurrentUserTests
{
    [Fact]
    public async Task GetAsync_WithAuthenticatedActiveUser_ReturnsCurrentUserContext()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        users.Add(user);
        FakeCurrentUserPrincipalAccessor principalAccessor = new();
        principalAccessor.SetUser(user);
        GetCurrentUser useCase = new(principalAccessor, users);

        Result<CurrentUserProfile, AuthenticationFailure> result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);

        CurrentUserProfile currentUser = result.Value;
        Assert.Equal(user.Id, currentUser.UserId);
        Assert.Equal("user@example.com", currentUser.Email);
        Assert.Equal("Example User", currentUser.Name);
        Assert.Equal(["User"], currentUser.Roles);
        Assert.False(currentUser.MustChangePasswordOnFirstLogin);
    }
}
