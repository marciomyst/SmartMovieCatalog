# Implementation Plan: Backend Authentication With JWT And Current User Context

**Branch**: `021-backend-authentication-with-jwt-and-current-user-context` | **Date**: 2026-05-03 | **Spec**: `specs/021-backend-authentication-with-jwt-and-current-user-context/spec.md`
**Input**: Feature specification from `specs/021-backend-authentication-with-jwt-and-current-user-context/spec.md`

## Summary

Implement backend-only local email/password authentication with JWT bearer access tokens and current-user lookup. The implementation will add a domain user aggregate, application authentication/current-user use cases, infrastructure persistence/password/JWT services, ASP.NET Core authentication middleware, explicit public API contracts, and focused backend tests. ADR 0002, ADR 0003, and ADR 0004 are accepted and authorize the required JWT, EF Core/PostgreSQL, and `ProblemDetails` decisions.

## Technical Context

**Language/Version**: C# / .NET 10 (`net10.0`), ASP.NET Core 10  
**Primary Dependencies**: Existing Clean Architecture projects; planned backend dependencies are `Microsoft.AspNetCore.Authentication.JwtBearer`, EF Core, Npgsql EF Core provider, and framework password hashing primitives without full ASP.NET Core Identity UI/user-management adoption  
**Storage**: PostgreSQL via EF Core in `SmartMovieCatalog.Infrastructure`, using `ConnectionStrings:DefaultConnection` and migrations owned by Infrastructure  
**Testing**: Backend test projects currently compile-only; implementation will deliberately add a .NET test framework, assertion support, and `Microsoft.AspNetCore.Mvc.Testing` for API tests  
**Target Platform**: ASP.NET Core API running locally, in Docker, and under the existing Linux container target  
**Project Type**: Backend web API inside a backend/frontend monorepo; no frontend implementation in this feature  
**Performance Goals**: Authentication and current-user requests should use indexed normalized-email/user-id lookups and avoid unnecessary database round trips  
**Constraints**: Clean Architecture dependency direction; no committed secrets; generic authentication failures; `UseAuthentication()` before `UseAuthorization()`; no refresh tokens, registration, password recovery, external identity provider, frontend auth, or TestContainers assumption  
**Scale/Scope**: First product backend security foundation: two API endpoints, one persisted user model, basic `Admin`/`User` role claims, development/test-only seed path

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Actual architecture source of truth: PASS. Plan follows the existing API/Application/Domain/Infrastructure/Contracts project split and accepted ADRs.
- Explicit boundaries: PASS. Domain model stays framework-free; Application owns use-case abstractions; Infrastructure owns EF Core/JWT/password implementations; Api owns HTTP/middleware.
- Contracts drive cross-boundary work: PASS. API request/response and error contracts are documented under `contracts/`.
- Security and secret hygiene: PASS. JWT signing key and database connection string are runtime configuration only; no secrets are committed in plan artifacts.
- Small, verified, reviewable changes: PASS. Scope is limited to backend authentication/current-user behavior plus tests and docs.
- Backend dependency direction: PASS. No Domain references to ASP.NET Core, EF Core, Contracts, or Infrastructure are planned.
- Do not invent infrastructure: PASS with accepted ADRs. EF Core/PostgreSQL and JWT are authorized by ADR 0003 and ADR 0002.

## Project Structure

### Documentation (this feature)

```text
specs/021-backend-authentication-with-jwt-and-current-user-context/
+-- plan.md
+-- research.md
+-- data-model.md
+-- quickstart.md
+-- contracts/
|   +-- auth.openapi.yaml
+-- tasks.md
```

### Source Code (repository root)

```text
backend/
+-- src/
|   +-- SmartMovieCatalog.Api/
|   |   +-- Controllers/
|   |   |   +-- AuthController.cs
|   |   +-- Program.cs
|   |   +-- SmartMovieCatalog.Api.http
|   +-- SmartMovieCatalog.Application/
|   |   +-- Abstractions/
|   |   |   +-- Authentication/
|   |   |   +-- Persistence/
|   |   |   +-- Time/
|   |   +-- Authentication/
|   +-- SmartMovieCatalog.Contracts/
|   |   +-- Authentication/
|   +-- SmartMovieCatalog.Domain/
|   |   +-- Users/
|   +-- SmartMovieCatalog.Infrastructure/
|       +-- Authentication/
|       +-- Persistence/
+-- tests/
    +-- SmartMovieCatalog.Application.Tests/
    +-- SmartMovieCatalog.Api.Tests/
    +-- SmartMovieCatalog.Infrastructure.Tests/
```

**Structure Decision**: Use the current Clean Architecture backend layout. Public HTTP DTOs live in Contracts, HTTP routing and response shaping live in Api, orchestration and abstractions live in Application, the user aggregate lives in Domain, and EF Core/JWT/password hashing/current request implementations live in Infrastructure. No frontend paths are in scope.

## Phase 0: Research

Research is captured in `specs/021-backend-authentication-with-jwt-and-current-user-context/research.md`.

Resolved decisions:

- Local email/password plus JWT bearer access tokens.
- EF Core/PostgreSQL persistence under Infrastructure.
- Domain user aggregate with infrastructure mappings.
- Application-layer authentication/current-user orchestration.
- ASP.NET Core `ProblemDetails` and `ValidationProblemDetails`.
- Framework password hashing primitives without adopting full ASP.NET Core Identity.
- Startup validation for JWT and persistence configuration.
- Deliberate backend test framework and focused API/application tests.

## Phase 1: Design And Contracts

Design artifacts:

- Data model: `specs/021-backend-authentication-with-jwt-and-current-user-context/data-model.md`
- API contract: `specs/021-backend-authentication-with-jwt-and-current-user-context/contracts/auth.openapi.yaml`
- Quickstart: `specs/021-backend-authentication-with-jwt-and-current-user-context/quickstart.md`

Contract highlights:

- `POST /api/auth/authenticate` accepts `{ email, password }`.
- Successful authentication returns `{ userId, email, accessToken, accessTokenExpiresAtUtc }`.
- `GET /api/auth/me` requires bearer authentication and returns `{ userId, email, name, roles, mustChangePasswordOnFirstLogin }`.
- Validation failures use `ValidationProblemDetails`; authentication failures use generic `ProblemDetails`.

## Post-Design Constitution Check

- Actual architecture source of truth: PASS. The design uses existing project boundaries and accepted ADRs.
- Explicit boundaries: PASS. Contracts do not leak persistence entities; Domain remains independent of EF Core and ASP.NET Core.
- Contracts drive cross-boundary work: PASS. API behavior, request/response shape, validation behavior, and error behavior are documented.
- Security and secret hygiene: PASS. Config examples use placeholders and user-secrets/environment variables only.
- Small, verified, reviewable changes: PASS. No frontend behavior, refresh tokens, registration, external identity providers, or broad authorization policies are included.
- Backend dependency direction: PASS. Infrastructure references Application/Domain; Api references Application/Infrastructure/Contracts.
- Do not invent infrastructure: PASS. Persistence and auth packages are justified by accepted ADRs and the spec.

## Complexity Tracking

No constitution violations require justification.
