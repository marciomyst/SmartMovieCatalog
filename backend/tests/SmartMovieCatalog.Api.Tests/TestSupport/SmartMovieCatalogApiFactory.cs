using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SmartMovieCatalog.Application.Abstractions.Authentication;
using SmartMovieCatalog.Application.Abstractions.Persistence;

namespace SmartMovieCatalog.Api.Tests.TestSupport;

public sealed class SmartMovieCatalogApiFactory : WebApplicationFactory<Program>
{
    public FakeUserRepository Users { get; } = new();

    public FakeMovieRepository Movies { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            var config = new Dictionary<string, string?>
            {
                ["Jwt:Issuer"] = TestJwtOptions.Issuer,
                ["Jwt:Audience"] = TestJwtOptions.Audience,
                ["Jwt:SigningKey"] = TestJwtOptions.SigningKey,
                ["Jwt:AccessTokenLifetimeMinutes"] = "60",
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=smart_movie_catalog_tests;Username=smartmovie;Password=placeholder"
            };

            configurationBuilder.AddInMemoryCollection(config);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IUserRepository>();
            services.RemoveAll<IMovieRepository>();
            services.RemoveAll<IPasswordHasher>();
            services.AddSingleton<IUserRepository>(Users);
            services.AddSingleton<IMovieRepository>(Movies);
            services.AddSingleton<IPasswordHasher, TestPasswordHasher>();
        });
    }
}
