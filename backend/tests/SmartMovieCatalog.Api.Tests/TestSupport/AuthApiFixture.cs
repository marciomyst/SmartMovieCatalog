using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Api.Tests.TestSupport;

public static class AuthApiFixture
{
    public const string Password = "Password123!";

    public static User ActiveUser(string email = "user@example.com")
    {
        TestPasswordHasher passwordHasher = new();
        User user = User.Create(
            UserId.New(),
            EmailAddress.Create(email),
            "Example User",
            "pending",
            [UserRole.Create(UserRole.User)],
            mustChangePasswordOnFirstLogin: false,
            new DateTimeOffset(2026, 5, 3, 1, 0, 0, TimeSpan.Zero));

        user.SetPasswordHash(passwordHasher.HashPassword(user, Password), new DateTimeOffset(2026, 5, 3, 1, 0, 0, TimeSpan.Zero));
        return user;
    }

    public static void SetBearerToken(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static string CreateToken(Guid userId, DateTimeOffset? expiresAtUtc = null, string? signingKey = null)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset notBefore = expiresAtUtc is { } expiresAt && expiresAt <= now
            ? expiresAt.AddMinutes(-5)
            : now.AddMinutes(-5);
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(signingKey ?? TestJwtOptions.SigningKey));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken token = new(
            issuer: TestJwtOptions.Issuer,
            audience: TestJwtOptions.Audience,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, UserRole.User)
            ],
            notBefore: notBefore.UtcDateTime,
            expires: (expiresAtUtc ?? now.AddMinutes(60)).UtcDateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
