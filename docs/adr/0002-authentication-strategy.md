# ADR 0002: Authentication Strategy

## Status
Accepted

## Context
Issue #21 introduced a backend-only authentication foundation for product API endpoints using local email/password credentials and JWT bearer access tokens.

The current feature scope is backend API only. Angular login/session UI, refresh tokens, registration, password recovery, external identity providers, tenancy, and granular authorization policies are out of scope for the initial implementation.

Authentication depends on persisted local users as accepted in `docs/adr/0003-database-strategy.md`.

## Decision
Use local authentication with email/password credentials and JWT bearer access tokens.

- Expose `POST /api/auth/authenticate` for credential authentication.
- Expose `GET /api/auth/me` for current authenticated user context.
- Store passwords only as secure password hashes. Plaintext passwords must never be stored, logged, returned, or persisted in configuration.
- Issue JWT bearer access tokens for successfully authenticated active users.
- Include the authenticated user identifier and basic roles in token claims.
- Reject authentication and current-user lookup for nonexistent, inactive, or removed users.
- Configure ASP.NET Core authentication with `AddAuthentication().AddJwtBearer(...)`.
- Configure middleware with `UseAuthentication()` before `UseAuthorization()`.
- Read JWT settings from environment variables, user-secrets, or another non-versioned configuration source. Do not place signing secrets in `appsettings*.json`, source files, Dockerfiles, frontend bundles, committed documentation, or Spec Kit artifacts.
- Validate required JWT settings at startup and fail closed when signing material or required validation settings are missing.

The initial JWT configuration must define:

- issuer;
- audience;
- signing key or equivalent signing credential source;
- access token lifetime;
- validation behavior for issuer, audience, signing key, and token lifetime.

Authorization is limited to authentication and basic role claims for this decision. Fine-grained authorization policies require a later ADR or explicit feature scope.

Frontend token storage and session behavior are intentionally not selected by this ADR because issue #21 excludes frontend work.

## Consequences
- Backend implementation may add ASP.NET Core JWT bearer authentication packages and configuration.
- Backend implementation may add application abstractions and infrastructure services for password hashing, JWT generation, and current-user access.
- API endpoints must return generic authentication failures for invalid credentials, nonexistent users, inactive users, and removed users to avoid account enumeration.
- Logs must not include passwords, password hashes, access tokens, signing keys, or raw authorization headers.
- Refresh tokens, registration, password recovery, external identity providers, tenancy, and frontend auth/session code remain out of scope until separate decisions or feature requests introduce them.

## Implementation Notes

- Auth HTTP endpoints are implemented as Minimal API feature slices under `backend/src/SmartMovieCatalog.Api/Features/Auth`.
- Public request/response DTOs are in `backend/src/SmartMovieCatalog.Contracts/Auth`.
- Application use cases remain in `backend/src/SmartMovieCatalog.Application/Features/Auth`.
- HTTP endpoints dispatch auth command/query messages through Wolverine as accepted in `docs/adr/0007-wolverine-cqrs-mediator.md`.
