using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartMovieCatalog.Domain.Users;
using SmartMovieCatalog.Infrastructure.Authentication;

namespace SmartMovieCatalog.Infrastructure.Tests.Authentication;

public sealed class JwtAccessTokenServiceTests
{
    [Fact]
    public void CreateAccessToken_WithUser_CreatesSignedTokenWithExpectedClaims()
    {
        JwtOptions options = new()
        {
            Issuer = "SmartMovieCatalog",
            Audience = "SmartMovieCatalog.Api",
            SigningKey = "0123456789abcdef0123456789abcdef",
            AccessTokenLifetimeMinutes = 60
        };
        JwtAccessTokenService service = new(Options.Create(options));
        DateTimeOffset issuedAtUtc = new(2026, 5, 3, 1, 0, 0, TimeSpan.Zero);
        User user = User.Create(
            UserId.New(),
            EmailAddress.Create("user@example.com"),
            "Example User",
            "hash",
            [UserRole.Create(UserRole.Admin), UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            issuedAtUtc);

        var accessToken = service.CreateAccessToken(user, issuedAtUtc);

        Assert.Equal(issuedAtUtc.AddMinutes(60), accessToken.ExpiresAtUtc);

        TokenValidationParameters validationParameters = options.CreateTokenValidationParameters();
        validationParameters.ValidateLifetime = false;
        ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(
            accessToken.Value,
            validationParameters,
            out SecurityToken securityToken);

        Assert.IsType<JwtSecurityToken>(securityToken);
        Assert.Equal(user.Id.ToString(), principal.FindFirstValue(ClaimTypes.NameIdentifier));
        Assert.Equal("user@example.com", principal.FindFirstValue(ClaimTypes.Email));
        Assert.Contains(principal.FindAll(ClaimTypes.Role), claim => claim.Value == UserRole.Admin);
        Assert.Contains(principal.FindAll(ClaimTypes.Role), claim => claim.Value == UserRole.User);
    }
}
