# Architecture

## Current State
AI Flix / SmartMovieCatalog is a small monorepo with two runtime surfaces:

- Backend: ASP.NET Core 10 in `backend/src/SmartMovieCatalog.Api`.
- Frontend: Angular 21 in `frontend`.

The backend uses a Clean Architecture project split and integrates the Angular SPA through ASP.NET Core SpaProxy.
Authentication and persistence are implemented: local email/password authentication issues JWT bearer access tokens, local users are stored with EF Core/PostgreSQL, and backend use cases are dispatched through Wolverine as an in-process CQRS mediator.

## Source Of Truth
- Solution: `SmartMovieCatalog.slnx`
- Backend project: `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`
- Application project: `backend/src/SmartMovieCatalog.Application/SmartMovieCatalog.Application.csproj`
- Domain project: `backend/src/SmartMovieCatalog.Domain/SmartMovieCatalog.Domain.csproj`
- Infrastructure project: `backend/src/SmartMovieCatalog.Infrastructure/SmartMovieCatalog.Infrastructure.csproj`
- Contracts project: `backend/src/SmartMovieCatalog.Contracts/SmartMovieCatalog.Contracts.csproj`
- Backend entry point: `backend/src/SmartMovieCatalog.Api/Program.cs`
- Backend HTTP features: `backend/src/SmartMovieCatalog.Api/Features`
- Frontend package manifest: `frontend/package.json`
- Frontend visual rules: `frontend/DESIGN.md`

## Boundaries
- Backend concerns stay in `backend/src/SmartMovieCatalog.Api`.
- Frontend concerns stay in `frontend`.
- Documentation stays in `docs`.
- Generated and vendor paths are not architectural input: `node_modules`, `bin`, `obj`, `dist`, `.vs`, `.angular/cache`.

## Architectural Constraints
- EF Core/PostgreSQL, JWT authentication, and Wolverine in-process CQRS are accepted for the implemented backend foundation.
- Do not assume Wolverine transports, background workers, distributed messaging infrastructure, external AI providers, or additional persistence stores until they exist in the codebase or are explicitly requested as an architecture change.
- Do not introduce new persistence providers, external services, or cross-process infrastructure without documenting the decision and validating its impact.
- Keep layer boundaries useful: add behavior to the layer that owns the responsibility, not to whichever project is convenient.

## Dependency Direction
Backend dependency direction:

- `SmartMovieCatalog.Api` references Application, Infrastructure, Contracts, and the Angular project for SPA integration.
- `SmartMovieCatalog.Infrastructure` references Application and Domain.
- `SmartMovieCatalog.Application` references Domain.
- `SmartMovieCatalog.Contracts` has no internal project dependencies.
- `SmartMovieCatalog.Domain` has no internal project dependencies.

Domain must not depend on ASP.NET Core, EF Core, Angular, or infrastructure concerns.

## Decision Records
Use `docs/adr` for material architecture decisions. Add an ADR when introducing a durable choice such as database provider, authentication model, message broker, API versioning strategy, deployment topology, or a layer split.

Current ADRs:

- `docs/adr/0001-architecture-style.md`
- `docs/adr/0002-authentication-strategy.md`
- `docs/adr/0003-database-strategy.md`
- `docs/adr/0004-api-error-contract.md`
- `docs/adr/0005-spec-kit-workflow.md`
- `docs/adr/0006-domain-aggregate-organization.md`
- `docs/adr/0007-wolverine-cqrs-mediator.md`
