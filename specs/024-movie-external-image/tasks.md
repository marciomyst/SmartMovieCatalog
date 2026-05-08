# Tasks: Movie External Identifier And Card Image

**Input**: Design documents from `specs/024-movie-external-image/`
**Prerequisites**: [plan.md](./plan.md), [spec.md](./spec.md), [research.md](./research.md), [data-model.md](./data-model.md), [contracts/create-movie.md](./contracts/create-movie.md), [quickstart.md](./quickstart.md)

**Tests**: Included because the specification requires focused coverage of movie rules, saved metadata, and public movie payloads.

**Organization**: Tasks are grouped by user story so the requested metadata support can be delivered first, then backward compatibility can be validated as a separate increment.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks.
- **[Story]**: Maps to a user story from [spec.md](./spec.md).
- Every task includes a concrete file path or project path.

## Phase 1: Setup

**Purpose**: Confirm the current movie create slice and migration baseline before editing.

- [x] T001 Inspect current movie aggregate and create pipeline in `backend/src/SmartMovieCatalog.Domain/Movies/Movie.cs`, `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieCommand.cs`, `backend/src/SmartMovieCatalog.Application/Features/Movies/CreatedMovie.cs`, and `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieHandler.cs`
- [x] T002 Inspect current public create movie contract and endpoint mapping in `backend/src/SmartMovieCatalog.Contracts/Movies/CreateMovieRequest.cs`, `backend/src/SmartMovieCatalog.Contracts/Movies/MovieResponse.cs`, `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`, and `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieRequestValidator.cs`
- [x] T003 Inspect current movie persistence mapping and migration snapshot in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/MovieConfiguration.cs` and `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/SmartMovieCatalogDbContextModelSnapshot.cs`

---

## Phase 2: Foundational

**Purpose**: No new shared infrastructure is required. The existing Clean Architecture movie slice, EF Core/PostgreSQL persistence, Wolverine dispatch, FluentValidation, and backend test projects are already present.

**Checkpoint**: Foundation is ready. User story implementation can begin.

---

## Phase 3: User Story 1 - Save TMDB Metadata With A Movie (Priority: P1) MVP

**Goal**: A movie can be saved with optional `externalId` and `image`, and the saved representation returns both values.

**Independent Test**: Create or store a movie with `externalId` and `image`; confirm both values are validated, persisted through the create pipeline, and returned in the created movie response.

### Tests for User Story 1

- [x] T004 [P] [US1] Add domain tests for positive `ExternalId`, non-positive `ExternalId`, trimmed relative `Image`, blank `Image` normalization, and non-relative `Image` rejection in `backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs`
- [x] T005 [P] [US1] Add application handler assertions for persisted and returned `ExternalId` and `Image` in `backend/tests/SmartMovieCatalog.Application.Tests/Movies/CreateMovieHandlerTests.cs`
- [x] T006 [P] [US1] Add API validator coverage for invalid non-positive `ExternalId` and non-relative `Image` in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieRequestValidatorTests.cs`
- [x] T007 [P] [US1] Add API contract assertions for `externalId` and `image` in the successful response body in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs`

### Implementation for User Story 1

- [x] T008 [US1] Add nullable `ExternalId` and `Image` properties, constructor parameters, create factory parameters, positive `ExternalId` validation, relative `Image` validation, and `Image` normalization in `backend/src/SmartMovieCatalog.Domain/Movies/Movie.cs`
- [x] T009 [P] [US1] Add optional `ExternalId` and `Image` fields to `CreateMovieCommand` and `CreatedMovie` in `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieCommand.cs` and `backend/src/SmartMovieCatalog.Application/Features/Movies/CreatedMovie.cs`
- [x] T010 [US1] Pass `ExternalId` and `Image` through movie creation and created result mapping in `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieHandler.cs`
- [x] T011 [P] [US1] Add optional `ExternalId` and `Image` fields to public movie DTOs in `backend/src/SmartMovieCatalog.Contracts/Movies/CreateMovieRequest.cs` and `backend/src/SmartMovieCatalog.Contracts/Movies/MovieResponse.cs`
- [x] T012 [US1] Map `ExternalId` and `Image` from request to command and from created result to response in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`
- [x] T013 [US1] Add FluentValidation rules requiring `ExternalId` greater than zero and `Image` as a relative path when supplied in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieRequestValidator.cs`
- [x] T014 [US1] Add nullable `ExternalId` and `Image` mapping with an appropriate max length for `Image` in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/MovieConfiguration.cs`
- [x] T015 [US1] Generate EF Core migration adding nullable `ExternalId` and `Image` columns under `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/`
- [x] T016 [US1] Run focused domain, application, and API tests for the movie metadata path using `backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj`, `backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj`, and `backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj`

**Checkpoint**: User Story 1 is functional when movie creation with `externalId` and relative `image` persists and returns both values, while non-positive `externalId` and non-relative `image` values are rejected.

---

## Phase 4: User Story 2 - Preserve Existing Movie Creation Behavior (Priority: P2)

**Goal**: Existing callers can still create movies without supplying TMDB metadata, and existing rows remain valid with absent metadata.

**Independent Test**: Send the existing valid movie creation shape without the new fields and confirm it still succeeds with `externalId` and `image` represented as absent.

### Tests for User Story 2

- [x] T017 [P] [US2] Add or update endpoint test proving the existing minimal create movie payload still succeeds without `ExternalId` and `Image` in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointTests.cs`
- [x] T018 [P] [US2] Add or update contract test proving omitted `externalId` and `image` return as null in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs`
- [x] T019 [P] [US2] Add or update domain test proving omitted `ExternalId` and omitted `Image` remain null in `backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs`

### Implementation for User Story 2

- [x] T020 [US2] Verify `Movie` default constructor and nullable fields support existing persisted rows without `ExternalId` or `Image` in `backend/src/SmartMovieCatalog.Domain/Movies/Movie.cs`
- [x] T021 [US2] Verify generated migration uses nullable columns without backfill or uniqueness constraints in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/`
- [x] T022 [US2] Run focused backward-compatibility tests using `backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj` and `backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj`

**Checkpoint**: User Story 2 is functional when old create payloads still succeed, omitted metadata is null, and the migration does not require backfilling existing movies.

---

## Phase 5: Polish & Cross-Cutting Concerns

**Purpose**: Documentation, final verification, and cleanup across both stories.

- [x] T023 [P] Update movie request/response examples and optional field documentation, including relative `image` path validation, in `docs/API.md`
- [x] T024 [P] Update movie domain decisions for optional TMDB `ExternalId`, optional relative `Image`, no absolute image URLs, and no uniqueness/provider integration in `docs/DOMAIN.md`
- [x] T025 Review implementation against out-of-scope constraints from `specs/024-movie-external-image/spec.md`, `specs/024-movie-external-image/research.md`, and `specs/024-movie-external-image/contracts/create-movie.md`
- [x] T026 Run solution build for cross-layer backend changes using `SmartMovieCatalog.slnx`
- [x] T027 Confirm no frontend, provider credential, image upload/download, absolute image URL acceptance, TMDB fetch, new dependency, or unrelated refactor changes were introduced under `frontend/`, `backend/src/SmartMovieCatalog.Api/`, and `backend/src/SmartMovieCatalog.Infrastructure/`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Setup; no code changes required.
- **User Story 1 (Phase 3)**: Starts after Setup/Foundation and is the MVP.
- **User Story 2 (Phase 4)**: Starts after User Story 1 fields exist because it validates optional/backward-compatible behavior of the same fields.
- **Polish (Phase 5)**: Depends on desired user stories being complete.

### User Story Dependencies

- **US1**: No dependency on other user stories. Delivers the core metadata capability.
- **US2**: Depends on US1 implementation because compatibility is evaluated against the new nullable metadata fields.

### Within User Story 1

- T004-T007 should be written first and fail before implementation.
- T008 must precede T010.
- T009 and T011 can proceed in parallel after T008 design is clear.
- T010 depends on T008 and T009.
- T012 depends on T009, T010, and T011.
- T013 can proceed after T011.
- T014 precedes T015.
- T016 runs after T008-T015.

### Within User Story 2

- T017-T019 should be written first and fail if compatibility is broken.
- T020 and T021 verify nullable behavior after US1 implementation and migration exist.
- T022 runs after T017-T021.

## Parallel Opportunities

- Setup inspections T001-T003 can be done by different people if needed.
- US1 tests T004-T007 touch different test files and can run in parallel.
- US1 DTO/command work T009 and T011 can run in parallel once field names are agreed.
- Documentation tasks T023 and T024 can run in parallel after implementation behavior is settled.
- US2 compatibility tests T017-T019 touch different test files and can run in parallel.

## Parallel Example: User Story 1

```text
Task: "T004 [US1] Add domain tests in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs"
Task: "T005 [US1] Add application handler assertions in backend/tests/SmartMovieCatalog.Application.Tests/Movies/CreateMovieHandlerTests.cs"
Task: "T006 [US1] Add API validator coverage in backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieRequestValidatorTests.cs"
Task: "T007 [US1] Add API contract assertions in backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs"
```

## Parallel Example: User Story 2

```text
Task: "T017 [US2] Add existing payload endpoint test in backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointTests.cs"
Task: "T018 [US2] Add omitted metadata contract test in backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs"
Task: "T019 [US2] Add omitted metadata domain test in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs"
```

## Implementation Strategy

### MVP First

1. Complete Phase 1 and Phase 2.
2. Complete Phase 3 for US1.
3. Stop and validate movie creation with supplied `externalId` and `image`.

### Incremental Delivery

1. Deliver US1 to persist and return optional TMDB metadata.
2. Deliver US2 to prove omitted metadata remains backward compatible.
3. Finish docs and full solution build.

### Verification Strategy

Run focused tests as each story completes, then the broader solution build because this change crosses Domain, Application, Contracts, Api, Infrastructure, and docs:

```powershell
dotnet test backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj
dotnet build SmartMovieCatalog.slnx
```
