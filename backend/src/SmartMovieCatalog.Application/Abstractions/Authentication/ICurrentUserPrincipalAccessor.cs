using SmartMovieCatalog.Domain.Users;

namespace SmartMovieCatalog.Application.Abstractions.Authentication;

public interface ICurrentUserPrincipalAccessor
{
    CurrentUserPrincipal? GetCurrentUserPrincipal();
}

public sealed record CurrentUserPrincipal(UserId UserId);
