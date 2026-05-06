# Implementation Plan: Create Movie

**Branch**: `develop` | **Date**: 2026-05-04 | **Spec**: `specs/011-create-movie/spec.md`  
**Input**: Feature specification from `specs/011-create-movie/spec.md`

**Note**: `.specify/scripts/powershell/setup-plan.ps1 -Json` failed because the repository is currently on `develop`, not a Spec Kit feature branch. This plan explicitly targets `specs/011-create-movie`, generated from GitHub issue #11.

## Summary

Implement a backend-first create movie vertical slice through `POST /api/movies`. The slice adds explicit movie API contracts, request validation, Application-layer command handling through the existing in-process Wolverine mediator, a minimal framework-free Domain movie aggregate, EF Core/PostgreSQL persistence, focused backend tests, and documentation updates. No authentication, frontend UI, AI, SignalR, semantic search, TMDb integration, duplicate detection, or distributed messaging is included.

## Technical Context

**Language/Version**: C# / .NET 10 (`net10.0`), ASP.NET Core 10  
**Primary Dependencies**: Existing ASP.NET Core Minimal APIs, FluentValidation, WolverineFx as existing in-process mediator, EF Core with Npgsql/PostgreSQL, xUnit backend tests, `Microsoft.AspNetCore.Mvc.Testing` for API tests  
**Storage**: Existing EF Core/PostgreSQL persistence in `SmartMovieCatalog.Infrastructure`, with migrations under `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/`  
**Testing**: Existing backend test projects under `backend/tests`: Domain, Application, Api, and Infrastructure tests using xUnit patterns  
**Target Platform**: ASP.NET Core API running locally, under test host, and in the existing backend container model  
**Project Type**: Backend web API inside a backend/frontend monorepo; Angular frontend is out of scope for this slice  
**Performance Goals**: Single create request should perform one movie insert with no external service calls; no explicit latency SLO is required for this internal first slice  
**Constraints**: Clean Architecture dependency direction; no new production dependencies unless unavoidable; no committed secrets; no persistence model leakage; no authentication requirement; no new Wolverine infrastructure, transports, background workers, or event-driven behavior  
**Scale/Scope**: One product endpoint, one movie aggregate, one repository abstraction/implementation, one database migration, focused tests, and docs

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- Actual architecture is the source of truth: PASS. Plan follows the observed API/Application/Domain/Infrastructure/Contracts split, Minimal API feature slices, existing EF Core/PostgreSQL persistence, and accepted Wolverine in-process mediator setup.
- Boundaries stay explicit: PASS. API owns HTTP binding, validation metadata, status codes, and response shaping; Contracts owns public DTOs; Application owns orchestration and persistence abstraction; Domain owns movie invariants; Infrastructure owns EF Core mapping, repository, and migration.
- Contracts drive cross-boundary work: PASS. API behavior, request/response shape, validation behavior, error behavior, and frontend non-scope are defined before implementation.
- Security and secret hygiene: PASS. No secrets or secret-bearing configuration are introduced; request data is treated as untrusted; no raw request body logging is planned.
- Small, verified, reviewable changes: PASS. Scope is a single create endpoint and supporting backend layers, without speculative AI/search/import/auth/frontend behavior.
- Backend dependency direction: PASS. Domain remains framework-free; Contracts remains independent; Infrastructure depends inward; Api composes and dispatches.
- Do not invent infrastructure: PASS. Persistence, EF Core/PostgreSQL, Wolverine mediator, and xUnit tests already exist in the codebase. The plan does not add new providers, transports, background workers, or external services.
- Frontend constitution: PASS by non-scope. No frontend files are planned.

## Project Structure

### Documentation (this feature)

```text
specs/011-create-movie/
├── plan.md
├── research.md
├── data-model.md
├── quickstart.md
├── contracts/
│   └── movies.openapi.yaml
└── tasks.md
```

### Source Code (repository root)

```text
backend/
├── src/
│   ├── SmartMovieCatalog.Api/
│   │   ├── Features/Movies/
│   │   │   ├── MoviesEndpoints.cs
│   │   │   └── CreateMovie/
│   │   │       ├── CreateMovieEndpoint.cs
│   │   │       └── CreateMovieRequestValidator.cs
│   │   └── Program.cs
│   ├── SmartMovieCatalog.Application/
│   │   ├── Abstractions/Persistence/IMovieRepository.cs
│   │   └── Features/Movies/
│   │       ├── CreateMovieCommand.cs
│   │       ├── CreateMovieHandler.cs
│   │       └── CreatedMovie.cs
│   ├── SmartMovieCatalog.Contracts/
│   │   └── Movies/
│   │       ├── CreateMovieRequest.cs
│   │       └── MovieResponse.cs
│   ├── SmartMovieCatalog.Domain/
│   │   └── Movies/
│   │       ├── Movie.cs
│   │       ├── MovieGenre.cs
│   │       └── MovieId.cs
│   └── SmartMovieCatalog.Infrastructure/
│       └── Persistence/
│           ├── Configurations/MovieConfiguration.cs
│           ├── EfMovieRepository.cs
│           ├── SmartMovieCatalogDbContext.cs
│           └── Migrations/
└── tests/
    ├── SmartMovieCatalog.Domain.Tests/Movies/
    ├── SmartMovieCatalog.Application.Tests/Movies/
    ├── SmartMovieCatalog.Api.Tests/Movies/
    └── SmartMovieCatalog.Infrastructure.Tests/Persistence/
```

**Structure Decision**: Use the current Clean Architecture backend layout. No frontend structure is added because the spec does not include an existing movie creation page.

## Phase 0: Research

Research output: `specs/011-create-movie/research.md`

Resolved decisions:

- Use explicit Contracts DTOs for `CreateMovieRequest` and `MovieResponse`.
- Use a minimal `Movie` aggregate under `Domain/Movies` with `MovieId` and genre normalization.
- Use a server-generated GUID public movie ID returned as a JSON string.
- Require only `title`, `releaseYear`, `countryCode`, and `originalLanguage`.
- Validate `releaseYear` from `1888` through the next calendar year.
- Validate `countryCode` as exactly two letters and uppercase-normalize it; trim and preserve `originalLanguage`.
- Allow duplicate movie metadata in this first slice.
- Persist genres as a normalized child collection/table for near-term filtering flexibility without leaking persistence shape through contracts.
- Return `201 Created` with body plus `Location: /api/movies/{id}`; defer the read endpoint.
- Use the existing Wolverine in-process mediator only; do not introduce CQRS architecture, transports, retries, sagas, or distributed messaging.

## Phase 1: Design And Contracts

Design artifacts:

- Data model: `specs/011-create-movie/data-model.md`
- API contract: `specs/011-create-movie/contracts/movies.openapi.yaml`
- Quickstart: `specs/011-create-movie/quickstart.md`

Contract highlights:

- `POST /api/movies` accepts JSON with required `title`, `releaseYear`, `countryCode`, and `originalLanguage`; optional `originalTitle`, `genres`, `director`, `synopsis`, `durationMinutes`, and `ageRating`.
- Successful creation returns `201 Created`, `Location: /api/movies/{id}`, and a `MovieResponse`.
- Validation failures return ASP.NET Core `ValidationProblemDetails` with status `400`.
- No authorization header is required.

## Post-Design Constitution Check

- Actual architecture is the source of truth: PASS. Design uses existing backend feature-slice, Application handler, Domain aggregate, Infrastructure persistence, and Contracts project boundaries.
- Boundaries stay explicit: PASS. Public DTOs remain separate from Domain and EF Core models; Domain remains independent of ASP.NET Core, EF Core, Contracts, and Infrastructure.
- Contracts drive cross-boundary work: PASS. The OpenAPI contract defines request fields, response fields, validation status, and no-auth behavior.
- Security and secret hygiene: PASS. No secrets are introduced; API input validation and safe error behavior are specified.
- Small, verified, reviewable changes: PASS. Design is limited to one endpoint and supporting backend layers.
- Backend dependency direction: PASS. Planned references preserve the current Clean Architecture direction.
- Do not invent infrastructure: PASS. No new durable architecture decisions require an ADR.

## Complexity Tracking

No constitution violations require justification.
