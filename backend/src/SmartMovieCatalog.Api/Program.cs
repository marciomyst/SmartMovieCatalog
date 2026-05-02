
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Infrastructure;

namespace SmartMovieCatalog.Api
{
    public class Program
    {
        private const string HealthCheckUrl = "http://127.0.0.1:8080/health";

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
                using var response = httpClient.GetAsync(HealthCheckUrl).GetAwaiter().GetResult();
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
    }
}
