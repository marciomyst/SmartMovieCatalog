using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class AuthenticateUserFailureTests
{
    [Fact]
    public async Task AuthenticateAsync_WithWrongPassword_ReturnsGenericFailure()
    {
        FakeUserRepository users = new();
        users.Add(TestUsers.ActiveUser());
        AuthenticateUser useCase = CreateUseCase(users);

        Result<AuthenticatedUser, AuthenticationFailure> result = await useCase.AuthenticateAsync("user@example.com", "wrong", CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.InvalidCredentials, result.Error);
    }

    [Fact]
    public async Task AuthenticateAsync_WithNonexistentUser_ReturnsGenericFailure()
    {
        AuthenticateUser useCase = CreateUseCase(new FakeUserRepository());

        Result<AuthenticatedUser, AuthenticationFailure> result = await useCase.AuthenticateAsync("missing@example.com", TestUsers.Password, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.InvalidCredentials, result.Error);
    }

    [Fact]
    public async Task AuthenticateAsync_WithInactiveUser_ReturnsGenericFailure()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        user.Deactivate(new DateTimeOffset(2026, 5, 3, 2, 0, 0, TimeSpan.Zero));
        users.Add(user);
        AuthenticateUser useCase = CreateUseCase(users);

        Result<AuthenticatedUser, AuthenticationFailure> result = await useCase.AuthenticateAsync("user@example.com", TestUsers.Password, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.InvalidCredentials, result.Error);
    }

    [Fact]
    public async Task AuthenticateAsync_WithRemovedUser_ReturnsGenericFailure()
    {
        FakeUserRepository users = new();
        var user = TestUsers.ActiveUser();
        user.Remove(new DateTimeOffset(2026, 5, 3, 2, 0, 0, TimeSpan.Zero));
        users.Add(user);
        AuthenticateUser useCase = CreateUseCase(users);

        Result<AuthenticatedUser, AuthenticationFailure> result = await useCase.AuthenticateAsync("user@example.com", TestUsers.Password, CancellationToken.None);

        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(AuthenticationFailure.InvalidCredentials, result.Error);
    }

    private static AuthenticateUser CreateUseCase(FakeUserRepository users)
    {
        return new AuthenticateUser(
            users,
            new FakePasswordHasher(),
            new FakeAccessTokenService(),
            new FakeClock());
    }
}
