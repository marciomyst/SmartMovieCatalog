using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartMovieCatalog.Infrastructure.Authentication;

namespace SmartMovieCatalog.Infrastructure.Tests.Authentication;

public sealed class HttpCurrentUserPrincipalAccessorTests
{
    [Fact]
    public void GetCurrentUserPrincipal_WithoutHttpContext_ReturnsNull()
    {
        HttpCurrentUserPrincipalAccessor accessor = new(new HttpContextAccessor());

        Assert.Null(accessor.GetCurrentUserPrincipal());
    }

    [Fact]
    public void GetCurrentUserPrincipal_WithUnauthenticatedPrincipal_ReturnsNull()
    {
        HttpContextAccessor httpContextAccessor = new()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(new ClaimsIdentity())
            }
        };
        HttpCurrentUserPrincipalAccessor accessor = new(httpContextAccessor);

        Assert.Null(accessor.GetCurrentUserPrincipal());
    }

    [Fact]
    public void GetCurrentUserPrincipal_WithInvalidUserIdClaim_ReturnsNull()
    {
        HttpContextAccessor httpContextAccessor = new()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = CreatePrincipal("not-a-guid")
            }
        };
        HttpCurrentUserPrincipalAccessor accessor = new(httpContextAccessor);

        Assert.Null(accessor.GetCurrentUserPrincipal());
    }

    [Fact]
    public void GetCurrentUserPrincipal_WithNameIdentifierClaim_ReturnsCurrentUser()
    {
        Guid userId = Guid.NewGuid();
        HttpContextAccessor httpContextAccessor = new()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = CreatePrincipal(userId.ToString())
            }
        };
        HttpCurrentUserPrincipalAccessor accessor = new(httpContextAccessor);

        var principal = accessor.GetCurrentUserPrincipal();

        Assert.NotNull(principal);
        Assert.Equal(userId, principal.UserId.Value);
    }

    private static ClaimsPrincipal CreatePrincipal(string userId)
    {
        return new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.NameIdentifier, userId)],
            authenticationType: "Test"));
    }
}
