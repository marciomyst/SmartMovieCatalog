# Implementation Tasks: Normalize Movie Genres

**Feature**: `025-normalize-movie-genres` | **Date**: 2026-05-07 | **Spec**: [spec.md](./spec.md) | **Plan**: [plan.md](./plan.md)
**Input**: Feature specification, implementation plan, data model, and contracts

## Summary

Normalize movie genres by introducing reusable catalog genres with stable identifiers, normalized names, and optional TMDB external identifiers. Movie creation remains contract-compatible with `genres: string[]`, while the backend resolves those names into reusable `Genre` records and stores only movie-to-genre associations in `MovieGenres`.

## Dependencies

- **US1** (P1): Reuse Catalog Genres Across Movies
  - No dependencies
- **US2** (P2): Keep Movie Genre Associations Accurate
  - Depends on US1 completion
- **US3** (P3): Preserve Existing Movie Genre Data
  - Depends on US1 and US2 completion

## Parallel Execution Opportunities

- **US1 Setup**: Domain entities and configurations can be implemented in parallel
- **US1 Implementation**: Repository, handler, and configuration tasks can run in parallel
- **US2**: Association logic and validation can be implemented independently
- **US3**: Migration and data preservation can be done after core functionality

## Implementation Strategy

**MVP Scope**: Complete US1 (core genre reuse) first, then US2 (accurate associations), then US3 (data preservation). Each user story is independently testable and provides incremental value.

---

## Phase 1: Setup

- [x] T001 Create Genre aggregate root in backend/src/SmartMovieCatalog.Domain/Movies/Genre.cs
- [x] T002 Create GenreId value object in backend/src/SmartMovieCatalog.Domain/Movies/GenreId.cs
- [x] T003 Create MovieGenre value object in backend/src/SmartMovieCatalog.Domain/Movies/MovieGenre.cs
- [x] T004 Update Movie entity to use MovieGenre associations in backend/src/SmartMovieCatalog.Domain/Movies/Movie.cs
- [x] T005 Create IGenreRepository interface in backend/src/SmartMovieCatalog.Application/Abstractions/Persistence/IGenreRepository.cs
- [x] T006 Update CreateMovieCommand to support genre resolution in backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieCommand.cs
- [x] T007 Update CreateMovieHandler to resolve genres in backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieHandler.cs

---

## Phase 2: Foundational

- [x] T008 Create GenreConfiguration for EF Core in backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/GenreConfiguration.cs
- [x] T009 Create MovieGenreConfiguration for EF Core in backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/MovieGenreConfiguration.cs
- [x] T010 Update MovieConfiguration to remove owned genres in backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/MovieConfiguration.cs
- [x] T011 Implement EfGenreRepository in backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfGenreRepository.cs
- [x] T012 Register IGenreRepository in DI container in backend/src/SmartMovieCatalog.Infrastructure/DependencyInjection.cs
- [x] T013 Add Genres DbSet to SmartMovieCatalogDbContext in backend/src/SmartMovieCatalog.Infrastructure/Persistence/SmartMovieCatalogDbContext.cs

---

## Phase 3: User Story 1 - Reuse Catalog Genres Across Movies

**Goal**: Enable saving movie genres as reusable catalog entries that can be shared by many movies.

**Independent Test Criteria**:
- Can save two movies with the same genre name and confirm both reference the same catalog genre
- Movie creation contract remains compatible (accepts `genres: string[]`)
- Genre names are normalized and deduplicated

- [x] T014 [US1] Implement genre name normalization logic in Genre.cs
- [x] T015 [US1] Implement genre creation and reuse logic in EfGenreRepository.cs
- [x] T016 [US1] Update CreateMovieHandler to resolve genre names to Genre entities
- [x] T017 [US1] Update Movie.ReplaceGenres to work with Genre entities instead of strings
- [x] T018 [US1] Add domain tests for Genre normalization in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/GenreTests.cs
- [x] T019 [US1] Add domain tests for MovieGenre associations in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieGenreTests.cs
- [x] T020 [US1] Add application tests for genre resolution in backend/tests/SmartMovieCatalog.Application.Tests/Movies/CreateMovieHandlerTests.cs
- [x] T021 [US1] Add infrastructure tests for EfGenreRepository in backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfGenreRepositoryTests.cs

---

## Phase 4: User Story 2 - Keep Movie Genre Associations Accurate

**Goal**: Ensure movie-to-genre associations represent only which genres belong to each movie without mixing relationship data with genre names.

**Independent Test Criteria**:
- Movie with multiple genres creates correct associations
- Duplicate genre names in request result in single association
- Associations link movie to reusable genres without duplicating names

- [x] T022 [US2] Implement duplicate genre prevention in CreateMovieHandler.cs
- [x] T023 [US2] Update Movie entity validation for genre associations
- [x] T024 [US2] Add application tests for association accuracy in backend/tests/SmartMovieCatalog.Application.Tests/Movies/CreateMovieHandlerTests.cs
- [x] T025 [US2] Add domain tests for Movie genre association rules in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieTests.cs

---

## Phase 5: User Story 3 - Preserve Existing Movie Genre Data

**Goal**: Transition existing movie genre data to the new normalized structure without losing information.

**Independent Test Criteria**:
- Existing movies retain their genre names after migration
- Distinct genre names become reusable catalog entries
- Multiple movies with same genre share the catalog entry

- [x] T026 [US3] Create EF Core migration for genre normalization in backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/
- [x] T027 [US3] Implement data migration logic to preserve existing genres
- [ ] T028 [US3] Add migration tests to verify data preservation in backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/MigrationTests.cs
- [x] T029 [US3] Update database bootstrapper if needed in backend/src/SmartMovieCatalog.Infrastructure/Persistence/DatabaseBootstrapper.cs

---

## Phase 6: Polish & Cross-Cutting Concerns

- [ ] T030 Update API documentation for genre domain rules in docs/API.md
- [ ] T031 Update domain documentation for genre concepts in docs/DOMAIN.md
- [ ] T032 Add contract tests for movie genre compatibility in backend/tests/SmartMovieCatalog.Api.Tests/Movies/MovieGenreContractTests.cs
- [ ] T033 Update code coverage configuration if needed in backend/tests/coverage.runsettings
- [x] T034 Run full test suite to ensure no regressions
- [x] T035 Update README or quickstart documentation in docs/ or frontend/

---

## Task Summary

- **Total Tasks**: 35
- **Setup Phase**: 7 tasks (T001-T007)
- **Foundational Phase**: 6 tasks (T008-T013)
- **US1 Phase**: 8 tasks (T014-T021)
- **US2 Phase**: 4 tasks (T022-T025)
- **US3 Phase**: 4 tasks (T026-T029)
- **Polish Phase**: 6 tasks (T030-T035)

**Parallel Opportunities**: 12 tasks marked with [P] can be implemented in parallel within their phases.

**MVP Recommendation**: Complete Phases 1-3 (US1) first for core genre reuse functionality, then add US2 and US3 for completeness.