using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartMovieCatalog.Infrastructure.Authentication;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string? Issuer { get; init; }

    [Required]
    public string? Audience { get; init; }

    [Required]
    public string? SigningKey { get; init; }

    [Range(1, 1440)]
    public int AccessTokenLifetimeMinutes { get; init; } = 60;

    public SymmetricSecurityKey CreateSecurityKey()
    {
        if (string.IsNullOrWhiteSpace(SigningKey))
        {
            throw new InvalidOperationException("JWT signing key is not configured.");
        }

        byte[] keyBytes = Encoding.UTF8.GetBytes(SigningKey);
        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException("JWT signing key must be at least 32 bytes.");
        }

        return new SymmetricSecurityKey(keyBytes);
    }

    public TokenValidationParameters CreateTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = Issuer,
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = CreateSecurityKey(),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };
    }
}
