# Implementation Plan: Normalize Movie Genres

**Branch**: `[025-normalize-movie-genres]` | **Date**: 2026-05-07 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/025-normalize-movie-genres/spec.md`

## Summary

Normalize movie genres by introducing reusable catalog genres with stable identifiers, normalized names, and optional TMDB external identifiers. Movie creation remains contract-compatible with `genres: string[]`, while the backend resolves those names into reusable `Genre` records and stores only movie-to-genre associations in `MovieGenres`.

## Technical Context

**Language/Version**: C# / .NET 10, ASP.NET Core 10  
**Primary Dependencies**: EF Core, Npgsql PostgreSQL provider, Wolverine in-process dispatch, FluentValidation  
**Storage**: PostgreSQL through the existing EF Core infrastructure  
**Testing**: xUnit backend tests with existing API/application/domain/infrastructure test projects  
**Target Platform**: Backend web API hosted by `SmartMovieCatalog.Api`  
**Project Type**: Backend feature in Clean Architecture monorepo  
**Performance Goals**: Resolve a small movie genre list during movie creation without extra per-genre save operations  
**Constraints**: Preserve current public movie contract and Clean Architecture dependency direction  
**Scale/Scope**: Movie creation slice only; no frontend, genre management API, recommendations, or provider synchronization

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

- PASS: Uses existing backend Clean Architecture projects and dependency direction.
- PASS: Uses existing EF Core/PostgreSQL persistence; no new provider or external service.
- PASS: Keeps HTTP endpoint contract stable and moves genre resolution into Application/Infrastructure.
- PASS: Does not introduce secrets, credentials, provider calls, auth changes, or frontend changes.
- PASS: Scope remains small and reviewable with focused backend tests and documentation updates.

## Project Structure

### Documentation (this feature)

```text
specs/025-normalize-movie-genres/
|-- plan.md
|-- research.md
|-- data-model.md
|-- quickstart.md
|-- contracts/
|   `-- movie-genres.md
`-- checklists/
    `-- requirements.md
```

### Source Code (repository root)

```text
backend/
|-- src/
|   |-- SmartMovieCatalog.Domain/Movies/
|   |-- SmartMovieCatalog.Application/Features/Movies/
|   |-- SmartMovieCatalog.Application/Abstractions/Persistence/
|   |-- SmartMovieCatalog.Infrastructure/Persistence/
|   |-- SmartMovieCatalog.Infrastructure/Persistence/Configurations/
|   |-- SmartMovieCatalog.Infrastructure/Persistence/Migrations/
|   |-- SmartMovieCatalog.Contracts/Movies/
|   `-- SmartMovieCatalog.Api/Features/Movies/
`-- tests/
    |-- SmartMovieCatalog.Domain.Tests/Movies/
    |-- SmartMovieCatalog.Application.Tests/Movies/
    |-- SmartMovieCatalog.Infrastructure.Tests/Persistence/
    `-- SmartMovieCatalog.Api.Tests/Movies/
```

**Structure Decision**: Implement this as a backend-only persistence/domain refactor inside the existing Clean Architecture projects. No new projects or production dependencies are required.

## Complexity Tracking

No constitution violations.
