# ADR 0004: API Error Contract

## Status
Accepted

## Context
The current API surface is scaffold-level, but issue #21 introduces the first product backend endpoints for authentication. These endpoints need a stable and documented error contract for validation failures, authentication failures, and future product API behavior.

ASP.NET Core already provides standard `ProblemDetails` and `ValidationProblemDetails` response types that align with HTTP semantics and OpenAPI tooling without requiring a custom error envelope.

## Decision
Use ASP.NET Core `ProblemDetails` and `ValidationProblemDetails` for product API endpoints.

- Use `ValidationProblemDetails` for request validation failures such as malformed or invalid `POST /api/auth/authenticate` input.
- Use `ProblemDetails` for common product endpoint failures such as unauthorized, forbidden, not found, conflict, and server error responses.
- Include the HTTP status code and a stable machine-readable problem `type` or `title` when practical.
- Include trace/correlation information through the standard `ProblemDetails.Extensions` mechanism when available and safe.
- Do not expose stack traces, exception types, connection strings, provider payloads, secrets, password values, password hashes, access tokens, signing keys, or internal implementation details.
- Authentication failures must remain generic and must not disclose whether an email exists, a password was wrong, or the user is inactive or removed.

Product endpoints should document expected `ProblemDetails` and `ValidationProblemDetails` responses in OpenAPI metadata where the API layer supports it.

## Consequences
- Product endpoints should use framework-standard error responses instead of `SmartMovieCatalog.Contracts.Common.ApiErrorResponse`.
- `ApiErrorResponse` may remain temporarily while scaffold or older code exists, but new product endpoints should not depend on it unless a later ADR changes this decision.
- API tests should assert status codes and relevant `ProblemDetails`/`ValidationProblemDetails` fields instead of custom error DTOs.
- Documentation for new endpoints must describe validation and common error responses using the accepted contract.
