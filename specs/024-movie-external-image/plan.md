# Implementation Plan: Movie External Identifier And Card Image

**Branch**: `024-movie-external-image` | **Date**: 2026-05-07 | **Spec**: [spec.md](./spec.md)  
**Input**: Feature specification from `specs/024-movie-external-image/spec.md`

## Summary

Add two optional movie metadata fields across the existing backend movie creation slice: `externalId` for the TMDB movie identifier and `image` for the relative card image path. The implementation stays within the current Clean Architecture structure: domain invariants on `Movie`, application create command/result propagation, public request/response contracts, Minimal API request validation/mapping, EF Core/PostgreSQL persistence mapping and migration, focused backend tests, and documentation updates. No TMDB network integration, provider credentials, image upload/download, absolute external image URLs, uniqueness rule, frontend work, or new infrastructure is introduced.

## Technical Context

**Language/Version**: C# / ASP.NET Core 10 backend  
**Primary Dependencies**: Existing Minimal API feature slices, Wolverine in-process command dispatch, FluentValidation, EF Core, Npgsql/PostgreSQL  
**Storage**: Existing EF Core/PostgreSQL persistence for `Movies`  
**Testing**: Existing xUnit backend test projects under `backend/tests`, including Domain, Application, and Api coverage  
**Target Platform**: Backend web API running through the existing ASP.NET Core API project  
**Project Type**: Monorepo web application; this feature is backend-only  
**Performance Goals**: No material runtime performance change; the create movie path should remain a single movie persistence operation  
**Constraints**: Optional fields must preserve existing valid create-movie payloads; no secrets, provider calls, image binary handling, or new services  
**Scale/Scope**: Narrow movie metadata extension for the existing create movie slice and persisted movie record shape

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- **Actual Architecture Is The Source Of Truth**: PASS. Plan uses implemented ASP.NET Core 10, Clean Architecture projects, Wolverine in-process dispatch, FluentValidation, EF Core/PostgreSQL, and existing backend test projects documented in repository context.
- **Boundaries Must Stay Explicit**: PASS. Changes are scoped to backend source, backend tests, docs, and Spec Kit artifacts. No generated/vendor paths are used.
- **Contracts Drive Cross-Boundary Work**: PASS. Public create movie request/response shape is documented in `contracts/create-movie.md`; no frontend change is included.
- **Security And Secret Hygiene**: PASS. The feature stores caller-supplied metadata only and explicitly excludes TMDB credentials, provider calls, image downloads, uploads, and binary content.
- **Small, Verified, Reviewable Changes**: PASS. No new dependencies or infrastructure. Verification uses focused backend tests plus `dotnet build SmartMovieCatalog.slnx` because the change crosses Domain, Application, Contracts, Api, Infrastructure, and tests.
- **Backend Clean Architecture Direction**: PASS. Domain owns invariants, Application propagates create orchestration data, Api owns HTTP validation/mapping, Infrastructure owns persistence mapping/migration, Contracts remain DTO-only.
- **No Unjustified Infrastructure**: PASS. Existing EF Core/PostgreSQL persistence is extended; no new provider, external service, background processing, or provider SDK is introduced.

## Project Structure

### Documentation (this feature)

```text
specs/024-movie-external-image/
|-- plan.md
|-- research.md
|-- data-model.md
|-- quickstart.md
|-- contracts/
|   `-- create-movie.md
|-- checklists/
|   `-- requirements.md
`-- spec.md
```

### Source Code (repository root)

```text
backend/
|-- src/
|   |-- SmartMovieCatalog.Domain/
|   |   `-- Movies/
|   |       `-- Movie.cs
|   |-- SmartMovieCatalog.Application/
|   |   `-- Features/Movies/
|   |       |-- CreateMovieCommand.cs
|   |       |-- CreatedMovie.cs
|   |       `-- CreateMovieHandler.cs
|   |-- SmartMovieCatalog.Contracts/
|   |   `-- Movies/
|   |       |-- CreateMovieRequest.cs
|   |       `-- MovieResponse.cs
|   |-- SmartMovieCatalog.Api/
|   |   `-- Features/Movies/CreateMovie/
|   |       |-- CreateMovieEndpoint.cs
|   |       `-- CreateMovieRequestValidator.cs
|   `-- SmartMovieCatalog.Infrastructure/
|       `-- Persistence/
|           |-- Configurations/MovieConfiguration.cs
|           `-- Migrations/
`-- tests/
    |-- SmartMovieCatalog.Domain.Tests/
    |   `-- Movies/MovieValidationTests.cs
    |-- SmartMovieCatalog.Application.Tests/
    |   `-- Movies/CreateMovieHandlerTests.cs
    `-- SmartMovieCatalog.Api.Tests/
        `-- Movies/
            |-- CreateMovieEndpointContractTests.cs
            |-- CreateMovieEndpointTests.cs
            `-- CreateMovieRequestValidatorTests.cs

docs/
|-- API.md
`-- DOMAIN.md
```

**Structure Decision**: Use the existing backend Clean Architecture layout and the existing movie create feature slice. Do not add projects, packages, frontend routes, external TMDB clients, file storage components, or provider abstractions.

## Phase 0 Research

Completed in [research.md](./research.md). Key outcomes:

- Store `externalId` as optional positive integer metadata on `Movie`.
- Store `image` as optional normalized relative path on `Movie`.
- Propagate both fields through create request, command, created result, response, persistence mapping, migration, and tests.
- Keep uniqueness, provider provenance model, TMDB fetching, and image storage out of scope.

## Phase 1 Design

Completed artifacts:

- [data-model.md](./data-model.md)
- [contracts/create-movie.md](./contracts/create-movie.md)
- [quickstart.md](./quickstart.md)

## Post-Design Constitution Check

- **Actual Architecture Is The Source Of Truth**: PASS. Design artifacts match the existing movie create path and persistence model.
- **Boundaries Must Stay Explicit**: PASS. Source layout remains backend-only; frontend and generated/vendor paths are untouched.
- **Contracts Drive Cross-Boundary Work**: PASS. Request/response changes are explicitly documented before tasks are generated.
- **Security And Secret Hygiene**: PASS. No provider credentials or secret-bearing configuration appear in the design.
- **Small, Verified, Reviewable Changes**: PASS. The plan is a narrow metadata extension with focused tests and documentation.
- **Backend Clean Architecture Direction**: PASS. Responsibilities remain in their existing layers.
- **No Unjustified Infrastructure**: PASS. Extends current EF Core/PostgreSQL mapping only.

## Complexity Tracking

No constitution violations or added complexity require justification.
