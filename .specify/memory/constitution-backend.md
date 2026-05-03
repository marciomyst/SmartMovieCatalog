# SmartMovieCatalog Backend Constitution

## Scope

Apply this file when Spec Kit work touches backend behavior, API contracts, C# code, ASP.NET Core runtime configuration, Clean Architecture layers, backend tests, Docker backend runtime behavior, security boundaries, persistence decisions, or server-side AI integration.

This file extends `.specify/memory/constitution.md`; it does not replace it.

## Backend Source Of Truth

- Solution: `SmartMovieCatalog.slnx`.
- API layer: `backend/src/SmartMovieCatalog.Api`.
- Application layer: `backend/src/SmartMovieCatalog.Application`.
- Domain layer: `backend/src/SmartMovieCatalog.Domain`.
- Infrastructure layer: `backend/src/SmartMovieCatalog.Infrastructure`.
- Public contracts: `backend/src/SmartMovieCatalog.Contracts`.
- Backend tests: `backend/tests`.
- API design rules: `docs/API.md`.
- Domain direction: `docs/DOMAIN.md`.
- Security rules: `docs/SECURITY.md`.
- Testing policy: `docs/TESTING.md`.

## Backend Principles

### I. Clean Architecture Dependency Direction

Respect the current dependency direction:

- `SmartMovieCatalog.Api` may reference Application, Infrastructure, Contracts, and the Angular project for SpaProxy integration.
- `SmartMovieCatalog.Infrastructure` may reference Application and Domain.
- `SmartMovieCatalog.Application` may reference Domain.
- `SmartMovieCatalog.Contracts` must not reference internal projects.
- `SmartMovieCatalog.Domain` must not reference internal projects or framework/infrastructure concerns.

Domain code must not depend on ASP.NET Core, EF Core, Angular, infrastructure, persistence, transport DTOs, or provider SDKs.

### II. HTTP Is An API Boundary, Not A Business Layer

Controllers and minimal API endpoints must handle HTTP concerns only: routing, model binding, status codes, authorization metadata, and response shaping.

Move business rules, orchestration, validation, and domain decisions out of controllers when behavior becomes non-trivial. Use DTOs for public input/output once an endpoint exposes product behavior beyond scaffold examples.

### III. Do Not Invent Infrastructure

Do not introduce persistence, EF Core, PostgreSQL assumptions, CQRS, Wolverine, background workers, messaging, authentication, authorization models, external providers, AI provider SDKs, or database-backed tests unless the user explicitly requests it or the existing codebase already contains the choice.

Material infrastructure choices require an ADR under `docs/adr` before implementation tasks proceed.

### IV. Domain Rules Must Be Explicit

Domain behavior belongs in the Domain or Application layer, not in UI logic, controller conditionals, database configuration, or provider payload assumptions.

Before persistence is introduced, define movie identity, uniqueness, metadata ownership, trust level, AI analysis provenance, regeneration/versioning rules, and authorization implications where applicable.

### V. Backend Code Quality

C# must be explicit, nullable-safe, async for I/O, and testable. Prefer simple constructs over speculative abstractions. `var` is acceptable only when the type is obvious, anonymous, or clearer due to long generic expressions.

Do not place secrets in `appsettings*.json`, Dockerfiles, source files, committed docs, or Spec Kit artifacts. Use environment variables or user-secrets for local secret material.

## Backend Verification

Use the narrowest command that validates the backend change:

- API-only or backend project changes: `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`.
- Cross-layer, project reference, contracts, or shared behavior changes: `dotnet build SmartMovieCatalog.slnx`.
- Backend tests may be added or run when an applicable test framework exists. Current backend test projects are scaffolds and must not imply xUnit, TestContainers, EF Core, or database-backed tests unless those dependencies are introduced deliberately.

## Backend Documentation

Update documentation when backend behavior, API contracts, domain constraints, security posture, testing strategy, runtime configuration, or architecture decisions change.

Use `docs/adr` for durable backend architecture decisions.

**Version**: 1.0.0 | **Ratified**: 2026-05-02 | **Last Amended**: 2026-05-02
