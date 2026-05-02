
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Infrastructure;

namespace SmartMovieCatalog.Api
{
    public static class Program
    {
        private static readonly Uri HealthCheckUri = BuildHealthCheckUri();

        public static int Main(string[] args)
        {
            if (args is ["--healthcheck"])
            {
                return RunHealthCheck();
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapHealthChecks("/health");

            app.MapFallbackToFile("/index.html");

            app.Run();

            return 0;
        }

        private static int RunHealthCheck()
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(2)
            };

            try
            {
                using var response = httpClient.GetAsync(HealthCheckUri).GetAwaiter().GetResult();
                return response.IsSuccessStatusCode ? 0 : 1;
            }
            catch (HttpRequestException)
            {
                return 1;
            }
            catch (TaskCanceledException)
            {
                return 1;
            }
        }

        private static Uri BuildHealthCheckUri()
        {
            return new UriBuilder
            {
                Scheme = Uri.UriSchemeHttp,
                Host = "127.0.0.1",
                Port = 8080,
                Path = "health"
            }.Uri;
        }
    }
}
