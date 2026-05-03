using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Infrastructure.Authentication;

public sealed class HttpCurrentUserPrincipalAccessor : ICurrentUserPrincipalAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCurrentUserPrincipalAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserPrincipal? GetCurrentUserPrincipal()
    {
        ClaimsPrincipal? principal = _httpContextAccessor.HttpContext?.User;
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        string? userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? principal.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);

        if (!Guid.TryParse(userIdClaim, out Guid userId))
        {
            return null;
        }

        return new CurrentUserPrincipal(UserId.From(userId));
    }
}
