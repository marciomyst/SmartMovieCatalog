using FluentValidation;
using SmartMovieCatalog.Contracts.Auth;

namespace SmartMovieCatalog.Api.Features.Auth.Authenticate;

public sealed class AuthenticateRequestValidator : AbstractValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidator()
    {
        RuleFor(request => request.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(request => request.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
