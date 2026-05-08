using FluentValidation;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Api.Features.Auth;
using SmartMovieCatalog.Api.Features.Auth.Authenticate;
using SmartMovieCatalog.Api.Features.Movies;
using SmartMovieCatalog.Api.Features.Movies.CreateMovie;
using SmartMovieCatalog.Api.Features.Movies.ListMovies;
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Contracts.Auth;
using SmartMovieCatalog.Contracts.Movies;
using SmartMovieCatalog.Infrastructure;
using SmartMovieCatalog.Infrastructure.Persistence;
using Wolverine;

namespace SmartMovieCatalog.Api
{
    public class Program
    {
        protected Program()
        {
        }

        public static int Main(string[] args)
        {
            if (args is ["--healthcheck"])
            {
                return HealthCheckRunner.Run();
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IValidator<AuthenticateRequest>, AuthenticateRequestValidator>();
            builder.Services.AddScoped<IValidator<CreateMovieRequest>, CreateMovieRequestValidator>();
            builder.Services.AddScoped<IValidator<ListMoviesRequest>, ListMoviesRequestValidator>();
            builder.Services.AddHealthChecks();

            builder.Services.AddOpenApi();

            builder.AddAuthConfiguration();
            builder.Host.UseWolverine(options =>
            {
                options.EnableRemoteInvocation = false;
                options.Discovery.IncludeAssembly(ApplicationReference.Assembly);
            });

            var app = builder.Build();

            if (!app.Environment.IsEnvironment("Testing"))
            {
                using IServiceScope scope = app.Services.CreateScope();
                DatabaseBootstrapper databaseBootstrapper = scope.ServiceProvider.GetRequiredService<DatabaseBootstrapper>();
                databaseBootstrapper.BootstrapAsync(CancellationToken.None).GetAwaiter().GetResult();
            }

            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.UseDefaultFiles();
                app.MapStaticAssets();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapAuthEndpoints();
            app.MapMovieEndpoints();

            app.MapHealthChecks("/health");

            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.MapFallbackToFile("/index.html");
            }

            app.Run();

            return 0;
        }
    }
}
