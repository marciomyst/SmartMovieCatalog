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

        CurrentUserResult result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.Succeeded);
        Assert.Equal(user.Id, result.UserId);
        Assert.Equal("user@example.com", result.Email);
        Assert.Equal("Example User", result.Name);
        Assert.Equal(["User"], result.Roles);
        Assert.False(result.MustChangePasswordOnFirstLogin);
    }
}
