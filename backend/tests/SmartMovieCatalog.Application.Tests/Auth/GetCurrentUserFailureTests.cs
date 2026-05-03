using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application;
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

        Result<CurrentUserProfile, AuthenticationFailure> result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.Unauthenticated, result.Error);
    }

    [Fact]
    public async Task GetAsync_WithMissingUser_ReturnsUnavailableFailure()
    {
        FakeCurrentUserPrincipalAccessor principalAccessor = new()
        {
            Principal = new CurrentUserPrincipal(UserId.New())
        };
        GetCurrentUser useCase = new(principalAccessor, new FakeUserRepository());

        Result<CurrentUserProfile, AuthenticationFailure> result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Error);
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

        Result<CurrentUserProfile, AuthenticationFailure> result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Error);
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

        Result<CurrentUserProfile, AuthenticationFailure> result = await useCase.GetAsync(CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.UserUnavailable, result.Error);
    }
}
