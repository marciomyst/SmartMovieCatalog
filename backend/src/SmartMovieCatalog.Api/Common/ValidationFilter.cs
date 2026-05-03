using FluentValidation;
using FluentValidation.Results;

namespace SmartMovieCatalog.Api.Common;

public sealed class ValidationFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        TRequest? request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                [string.Empty] = ["A request body is required."]
            });
        }

        IValidator<TRequest> validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<TRequest>>();
        ValidationResult validationResult = await validator.ValidateAsync(
            request,
            context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(ToValidationProblemErrors(validationResult));
        }

        return await next(context);
    }

    private static Dictionary<string, string[]> ToValidationProblemErrors(ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
