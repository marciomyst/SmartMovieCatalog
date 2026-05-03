using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using SmartMovieCatalog.Infrastructure.Authentication;

namespace SmartMovieCatalog.Api.Common;

public static class AuthConfigurationExtensions
{
    public static WebApplicationBuilder AddAuthConfiguration(this WebApplicationBuilder builder)
    {
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
                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(problemDetails),
                            context.HttpContext.RequestAborted);
                    }
                };
            });

        builder.Services.AddAuthorization();

        return builder;
    }
}
