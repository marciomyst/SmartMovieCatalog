using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartMovieCatalog.Api.Common;
using SmartMovieCatalog.Application;
using SmartMovieCatalog.Infrastructure;
using SmartMovieCatalog.Infrastructure.Authentication;

namespace SmartMovieCatalog.Api
{
    public class Program
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
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.AddHealthChecks();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    JwtOptions jwtOptions = builder.Configuration
                        .GetSection(JwtOptions.SectionName)
                        .Get<JwtOptions>() ?? new JwtOptions();

                    options.TokenValidationParameters = jwtOptions.CreateTokenValidationParameters();
                    options.Events = new JwtBearerEvents
                    {
                        OnChallenge = async context =>
                        {
                            if (context.Response.HasStarted)
                            {
                                return;
                            }

                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/problem+json";

                            ProblemDetails problemDetails = AuthProblemDetails.Unauthorized(context.HttpContext);
                            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails), context.HttpContext.RequestAborted);
                        }
                    };
                });
            builder.Services.AddAuthorization();

            var app = builder.Build();

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


            app.MapControllers();

            app.MapHealthChecks("/health");

            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.MapFallbackToFile("/index.html");
            }

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
