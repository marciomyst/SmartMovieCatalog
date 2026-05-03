using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application.Abstractions.Persistence;
using SmartMovieCatalog.Application.Abstractions.Time;
using SmartMovieCatalog.Infrastructure.Authentication;
using SmartMovieCatalog.Infrastructure.Persistence;

namespace SmartMovieCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.SigningKey) && options.SigningKey.Length >= 32, "Jwt:SigningKey must be at least 32 characters.")
            .ValidateOnStart();

        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .Validate(options => !string.IsNullOrWhiteSpace(options.DefaultConnection), "ConnectionStrings:DefaultConnection is required.")
            .ValidateOnStart();

        services.AddDbContext<SmartMovieCatalogDbContext>((serviceProvider, options) =>
        {
            DatabaseOptions databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(databaseOptions.DefaultConnection);
        });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserRepository, EfUserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IAccessTokenService, JwtAccessTokenService>();
        services.AddScoped<ICurrentUserPrincipalAccessor, HttpCurrentUserPrincipalAccessor>();
        services.AddSingleton<IClock, SystemClock>();
        services.AddScoped<AdminUserSeeder>();
        services.AddScoped<DatabaseBootstrapper>();

        return services;
    }
}
