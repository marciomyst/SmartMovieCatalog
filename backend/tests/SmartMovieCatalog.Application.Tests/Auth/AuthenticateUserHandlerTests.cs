using SmartMovieCatalog.Application;
using SmartMovieCatalog.Application.Features.Auth;
using SmartMovieCatalog.Application.Tests.TestSupport;

namespace SmartMovieCatalog.Application.Tests.Auth;

public sealed class AuthenticateUserHandlerTests
{
    [Fact]
    public async Task Handle_DispatchesAuthenticateUseCase()
    {
        FakeUserRepository users = new();
        users.Add(TestUsers.ActiveUser());
        AuthenticateUser useCase = new(
            users,
            new FakePasswordHasher(),
            new FakeAccessTokenService(),
            new FakeClock());
        AuthenticateUserHandler handler = new(useCase);

        Result<AuthenticatedUser, AuthenticationFailure> result = await handler.Handle(
            new AuthenticateUserCommand("user@example.com", TestUsers.Password),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("user@example.com", result.Value.Email);
    }
}
