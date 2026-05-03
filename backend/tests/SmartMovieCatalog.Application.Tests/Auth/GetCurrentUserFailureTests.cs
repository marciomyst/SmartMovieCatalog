using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class GetCurrentUserFailureTests
{
    [Fact]
    public async Task GetAsync_WithoutPrincipal_ReturnsUnauthenticatedFailure()
    {
        GetCurrentUser useCase = new(new FakeCurrentUserPrincipalAccessor(), new FakeUserRepository());

        CurrentUserResult result = await useCase.GetAsync(CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(AuthenticationFailure.Unauthenticated, result.Failure);
    }

    [Fact]
    public async Task GetAsync_WithMissingUser_ReturnsUnavailableFailure()
    {
        FakeCurrentUserPrincipalAccessor principalAccessor = new()
        {
            Principal = new CurrentUserPrincipal(UserId.New())
        };
        GetCurrentUser useCase = new(principalAccessor, new FakeUserRepository());

        CurrentUserResult result = await useCase.GetAsync(CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Failure);
    }

    [Fact]
    public async Task GetAsync_WithInactiveUser_ReturnsUnavailableFailure()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        user.Deactivate(new DateTimeOffset(2026, 5, 3, 2, 0, 0, TimeSpan.Zero));
        users.Add(user);
        FakeCurrentUserPrincipalAccessor principalAccessor = new();
        principalAccessor.SetUser(user);
        GetCurrentUser useCase = new(principalAccessor, users);

        CurrentUserResult result = await useCase.GetAsync(CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Failure);
    }

    [Fact]
    public async Task GetAsync_WithRemovedUser_ReturnsUnavailableFailure()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        user.Remove(new DateTimeOffset(2026, 5, 3, 2, 0, 0, TimeSpan.Zero));
        users.Add(user);
        FakeCurrentUserPrincipalAccessor principalAccessor = new();
        principalAccessor.SetUser(user);
        GetCurrentUser useCase = new(principalAccessor, users);

        CurrentUserResult result = await useCase.GetAsync(CancellationToken.None);

        Assert.False(result.Succeeded);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Failure);
    }
}
