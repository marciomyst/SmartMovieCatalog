# Feature Specification

## Feature Summary

Implement backend-only authentication for SmartMovieCatalog using local email/password credentials and JWT bearer tokens. The feature adds `POST /api/auth/authenticate` for login and `GET /api/auth/me` for retrieving the authenticated user's current identity context.

The implementation must adapt the authentication design referenced from `MercaSafra` to this repository's current ASP.NET Core 10 Clean Architecture structure.

## Clarifications

### Session 2026-05-03

- Q: How should the first local user be provisioned without a registration flow? → A: Seed only in Development/Test; production uses operational/manual insertion outside the application.
- Q: What should the initial role model be for users and tokens? → A: Persist roles on the user; initial roles are `Admin` and `User`.
- Q: What should the JWT configuration names and initial access-token lifetime be? → A: Use `Jwt:Issuer`, `Jwt:Audience`, `Jwt:SigningKey`, and `Jwt:AccessTokenLifetimeMinutes`; default lifetime is 60 minutes; signing-key rotation is out of scope for this issue.
- Q: What error contract should authentication endpoints use? → A: Use ASP.NET Core `ProblemDetails` and `ValidationProblemDetails`.
- Q: How should a removed user be represented in this feature? → A: Use persisted soft delete with `RemovedAtUtc`; removed users are rejected.

## Problem Statement

The backend currently exposes only scaffold-level API behavior and has no authentication, user persistence, token issuance, or current-user context. Product API work needs a secure authenticated-user foundation before user-specific catalog behavior can be implemented.

## Goals

- Accept and document the local JWT authentication strategy in `docs/adr/0002-authentication-strategy.md`.
- Accept and document the EF Core/PostgreSQL persistence strategy in `docs/adr/0003-database-strategy.md`.
- Define or update the API error contract in `docs/adr/0004-api-error-contract.md`.
- Add a backend user domain model or aggregate with identity, email, name, password hash metadata, active/removal state, roles, and first-login password-change state.
- Add application abstractions and use cases for authentication and current-user lookup.
- Add infrastructure implementations for user persistence, password hashing, JWT generation, and current-user access.
- Configure ASP.NET Core JWT bearer authentication and ensure `UseAuthentication()` runs before `UseAuthorization()`.
- Expose backend API contracts for authentication and current-user responses.
- Add focused backend tests for application behavior and API endpoints.
- Update backend/API/security documentation and the API HTTP scratch file.

## Non-Goals

- No Angular login UI, session UI, route guards, or frontend API service changes.
- No refresh tokens.
- No user registration flow.
- No password recovery.
- No external identity provider.
- No organization or tenant model.
- No granular authorization beyond basic roles included in the token.

## User Stories

- As an API client, I can submit a valid email and password to receive a JWT access token and token expiry time.
- As an API client, I receive `401 Unauthorized` when credentials are invalid, the user does not exist, or the user is inactive.
- As an authenticated API client, I can call `GET /api/auth/me` to retrieve the current user's identity, roles, and first-login password-change state.
- As an unauthenticated API client, I receive `401 Unauthorized` when calling `GET /api/auth/me` without a valid bearer token.
- As the backend, I can reject tokens that point to removed, missing, or inactive users.

## Functional Requirements

- `POST /api/auth/authenticate` MUST accept a JSON body with `email` and `password`.
- `POST /api/auth/authenticate` MUST validate request shape and return `400 Bad Request` for invalid input.
- `POST /api/auth/authenticate` MUST return `401 Unauthorized` for invalid credentials, nonexistent users, inactive users, or removed users.
- `POST /api/auth/authenticate` MUST return `200 OK` with `{ userId, email, accessToken, accessTokenExpiresAtUtc }` when authentication succeeds.
- `GET /api/auth/me` MUST require a bearer token.
- `GET /api/auth/me` MUST return `401 Unauthorized` for missing, invalid, expired, or otherwise rejected tokens.
- `GET /api/auth/me` MUST verify that the token subject maps to an existing active, non-removed user.
- `GET /api/auth/me` MUST return `200 OK` with `{ userId, email, name, roles, mustChangePasswordOnFirstLogin }` for a valid authenticated user.
- JWT signing material and database connection strings MUST come from environment variables or user-secrets, not committed configuration.
- Passwords MUST be stored only as secure password hashes, never as plaintext.
- Error responses for product auth endpoints MUST use ASP.NET Core `ProblemDetails` for common errors and `ValidationProblemDetails` for validation errors.
- OpenAPI metadata and `SmartMovieCatalog.Api.http` MUST be updated for the new endpoints.
- The WeatherForecast scaffold MAY be removed once these real product endpoints are introduced.
- Non-test API startup MAY apply EF Core migrations and seed an optional admin user only when admin seed credentials are supplied from non-versioned configuration.
- User roles MUST be persisted as part of user state and emitted as JWT role claims; the initial role vocabulary is `Admin` and `User`.
- JWT configuration MUST use `Jwt:Issuer`, `Jwt:Audience`, `Jwt:SigningKey`, and `Jwt:AccessTokenLifetimeMinutes`; the initial access token lifetime is 60 minutes.
- Removed users MUST be represented by persisted soft delete using `RemovedAtUtc`; authentication and current-user lookup MUST reject users where `RemovedAtUtc` is set.

## Acceptance Criteria

- `POST /api/auth/authenticate`
  - Request: `{ "email": "user@example.com", "password": "Password123!" }`
  - `200 OK`: `{ userId, email, accessToken, accessTokenExpiresAtUtc }`
  - `400 Bad Request`: invalid input as `ValidationProblemDetails`
  - `401 Unauthorized`: invalid credentials, nonexistent user, or inactive user as `ProblemDetails`
- `GET /api/auth/me`
  - Requires bearer token.
  - `200 OK`: `{ userId, email, name, roles, mustChangePasswordOnFirstLogin }`
  - `401 Unauthorized`: missing/invalid token or nonexistent/inactive user as `ProblemDetails`
- Application tests cover:
  - valid authentication returns a token;
  - invalid password returns an unauthenticated failure;
  - nonexistent/inactive user does not authenticate;
  - current context requires an authenticated user;
  - current context rejects a removed/inactive user.
- API tests cover:
  - `POST /api/auth/authenticate` returns `200`, `400`, and `401` in the expected cases;
  - `GET /api/auth/me` returns `200` with a token and `401` without a token.
- Final verification includes `dotnet build SmartMovieCatalog.slnx` and the added backend tests.

## Edge Cases

- Email casing and whitespace must not allow duplicate identities or failed lookup for otherwise equivalent emails.
- Missing, malformed, expired, or incorrectly signed tokens must return `401 Unauthorized`.
- A token for a user that was deactivated or soft-deleted after issuance must be rejected by `GET /api/auth/me`.
- Authentication failure responses must not disclose whether the email exists.
- Validation errors must not echo secrets or raw passwords.
- JWT configuration missing or invalid at startup must fail safely rather than issuing unusable or unsigned tokens.
- JWT signing-key rotation is out of scope for this issue; the implementation must not imply automatic rotation support.
- Database connectivity failures must not expose connection details in API responses.

## Clarifications Needed

None identified.

## Assumptions

- PostgreSQL is acceptable because `docker-compose.yml` already provides a PostgreSQL service and `ConnectionStrings__DefaultConnection` for the API container.
- JWT secrets and database credentials will be supplied through environment variables or user-secrets.
- Admin seed credentials will be supplied only through non-versioned configuration and will not be committed to source files, Dockerfiles, migrations, or documentation.
- Role assignment is part of persisted user state; authorization remains limited to basic role claims until a later feature adds policies.
- Removed user state is represented by a nullable `RemovedAtUtc` timestamp.
- EF Core and Npgsql are allowed by accepted ADR 0003.
- JWT authentication packages are allowed by accepted ADR 0002.
- Backend test infrastructure may need a deliberate test framework package because current backend test projects are scaffold projects without a test framework.

## Dependencies

- `docs/adr/0002-authentication-strategy.md` must be accepted with local email/password plus JWT bearer authentication.
- `docs/adr/0003-database-strategy.md` must be accepted with EF Core, PostgreSQL, migration strategy, and local development configuration.
- `docs/adr/0004-api-error-contract.md` must be accepted with ASP.NET Core `ProblemDetails` and `ValidationProblemDetails` for product endpoints.
- ASP.NET Core authentication/authorization middleware configuration.
- EF Core provider for PostgreSQL if accepted by ADR 0003.
- A password hashing implementation with appropriate algorithm metadata.
- A backend test framework and API testing approach if endpoint tests are implemented.

## Risks

- Authentication and persistence are material architecture additions; implementing them before ADRs are accepted would violate repository governance.
- Production user provisioning depends on operational/manual insertion or the optional admin seed applied after migrations from non-versioned configuration; missing provisioning data will prevent real production logins.
- Poor JWT configuration validation could lead to insecure deployments or runtime failures.
- API tests may require additional dependencies and test-host setup not currently present in backend test projects.
- Introducing persistence without migration discipline may create unstable local and deployment behavior.

## Out of Scope

- Frontend login/session implementation.
- Refresh-token issuance, storage, revocation, or rotation.
- Self-service user registration.
- Password reset and email workflows.
- External identity providers.
- Multi-tenant ownership model.
- Fine-grained authorization policies beyond basic role claims.
