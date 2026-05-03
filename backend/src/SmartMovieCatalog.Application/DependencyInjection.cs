using Microsoft.Extensions.DependencyInjection;
using SmartMovieCatalog.Application.Features.Auth;

namespace SmartMovieCatalog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthenticateUser>();
        services.AddScoped<GetCurrentUser>();

        return services;
    }
}
