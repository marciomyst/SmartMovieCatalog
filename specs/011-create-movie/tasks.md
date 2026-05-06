# Tasks: Create Movie

**Input**: Design documents from `specs/011-create-movie/`  
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/movies.openapi.yaml`, `quickstart.md`  
**Prerequisite Note**: `.specify/scripts/powershell/check-prerequisites.ps1 -Json` failed because the current branch is `develop`, not a Spec Kit feature branch. These tasks target the explicit feature directory `specs/011-create-movie`.  
**Tests**: Included because the specification requires focused backend tests for domain, application, API, and persistence behavior.  
**Organization**: Tasks are grouped by independently testable user story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on incomplete tasks in the same phase
- **[Story]**: User story label for traceability
- Every task includes exact file paths

## User Stories

- **US1 (P1)**: As an API client, I can submit basic movie metadata and create a movie in the catalog.
- **US2 (P2)**: As an API client, I receive a consistent validation response when required or malformed input is submitted.
- **US3 (P3)**: As a maintainer, I can review the create movie behavior through clear contracts, architecture boundaries, persistence mapping, tests, and documentation.

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare source and test locations for the backend create movie slice.

- [X] T001 Create movie contracts folder in `backend/src/SmartMovieCatalog.Contracts/Movies/`
- [X] T002 Create movie API feature folders in `backend/src/SmartMovieCatalog.Api/Features/Movies/` and `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/`
- [X] T003 Create movie application feature folder in `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [X] T004 Create movie domain folder in `backend/src/SmartMovieCatalog.Domain/Movies/`
- [X] T005 Create movie test folders in `backend/tests/SmartMovieCatalog.Domain.Tests/Movies/`, `backend/tests/SmartMovieCatalog.Application.Tests/Movies/`, `backend/tests/SmartMovieCatalog.Api.Tests/Movies/`, and `backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Define shared contracts, domain model, repository abstraction, and persistence mapping required by all user stories.

**Critical**: No user story work can begin until this phase is complete.

- [X] T006 [P] Add `CreateMovieRequest` DTO matching `specs/011-create-movie/contracts/movies.openapi.yaml` in `backend/src/SmartMovieCatalog.Contracts/Movies/CreateMovieRequest.cs`
- [X] T007 [P] Add `MovieResponse` DTO matching `specs/011-create-movie/contracts/movies.openapi.yaml` in `backend/src/SmartMovieCatalog.Contracts/Movies/MovieResponse.cs`
- [X] T008 [P] Add server-generated GUID `MovieId` value object in `backend/src/SmartMovieCatalog.Domain/Movies/MovieId.cs`
- [X] T009 [P] Add trimmed non-blank `MovieGenre` value object in `backend/src/SmartMovieCatalog.Domain/Movies/MovieGenre.cs`
- [X] T010 Add `Movie` aggregate with required fields, optional metadata, duplicate-allowed identity, and normalization rules in `backend/src/SmartMovieCatalog.Domain/Movies/Movie.cs`
- [X] T011 Add `IMovieRepository` create abstraction in `backend/src/SmartMovieCatalog.Application/Abstractions/Persistence/IMovieRepository.cs`
- [X] T012 Add EF Core `MovieConfiguration` with normalized genre child collection mapping in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/MovieConfiguration.cs`
- [X] T013 Add `Movies` DbSet to `backend/src/SmartMovieCatalog.Infrastructure/Persistence/SmartMovieCatalogDbContext.cs`
- [X] T014 Add `EfMovieRepository` implementation in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`
- [X] T015 Register `IMovieRepository` to `EfMovieRepository` in `backend/src/SmartMovieCatalog.Infrastructure/DependencyInjection.cs`

**Checkpoint**: Foundation ready. Contracts, Domain movie model, repository abstraction, EF mapping, DbSet, repository implementation, and DI are in place.

---

## Phase 3: User Story 1 - Create Movie Successfully (Priority: P1) MVP

**Goal**: Valid movie metadata can be submitted to `POST /api/movies` without authentication and persisted, returning `201 Created`, a GUID movie ID, a normalized response body, and `Location: /api/movies/{id}`.

**Independent Test**: Submit the `quickstart.md` valid payload with lowercase `countryCode`; expect `201 Created`, `Location: /api/movies/{id}`, a GUID string `id`, `countryCode: "BR"`, no auth requirement, and persisted movie data with normalized genres.

### Tests for User Story 1

- [X] T016 [P] [US1] Add domain tests for valid movie creation, GUID identity, field trimming, country uppercase normalization, and optional metadata in `backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieTests.cs`
- [X] T017 [P] [US1] Add application fake movie repository in `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeMovieRepository.cs`
- [X] T018 [US1] Add application tests for successful create movie command using `IClock` and fake repository in `backend/tests/SmartMovieCatalog.Application.Tests/Movies/CreateMovieHandlerTests.cs`
- [X] T019 [P] [US1] Add API fake movie repository support in `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/FakeMovieRepository.cs`
- [X] T020 [US1] Update API test factory to replace `IMovieRepository` in `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/SmartMovieCatalogApiFactory.cs`
- [X] T021 [US1] Add API test for valid `POST /api/movies` returning `201 Created`, `Location`, GUID `id`, and no auth requirement in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointTests.cs`
- [X] T022 [US1] Add infrastructure persistence test for saving and reading a movie with normalized genres in `backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfMovieRepositoryTests.cs`

### Implementation for User Story 1

- [X] T023 [US1] Add `CreateMovieCommand` with required and optional metadata fields in `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieCommand.cs`
- [X] T024 [US1] Add `CreatedMovie` result model mirroring `MovieResponse` data in `backend/src/SmartMovieCatalog.Application/Features/Movies/CreatedMovie.cs`
- [X] T025 [US1] Add `CreateMovieHandler` that creates a `Movie`, uses `IClock` for `createdAtUtc`, and persists through `IMovieRepository` in `backend/src/SmartMovieCatalog.Application/Features/Movies/CreateMovieHandler.cs`
- [X] T026 [US1] Add `MoviesEndpoints` mapper in `backend/src/SmartMovieCatalog.Api/Features/Movies/MoviesEndpoints.cs`
- [X] T027 [US1] Add `CreateMovieEndpoint` that dispatches through `IMessageBus.InvokeAsync` and returns `Results.Created($"/api/movies/{id}", body)` in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`
- [X] T028 [US1] Map movie endpoints in the API host in `backend/src/SmartMovieCatalog.Api/Program.cs`
- [X] T029 [US1] Generate EF Core migration for `Movies` and normalized movie genres under `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/`

**Checkpoint**: User Story 1 is functional and testable independently as the MVP.

---

## Phase 4: User Story 2 - Validate Invalid Movie Input (Priority: P2)

**Goal**: Invalid or incomplete movie input returns `400 Bad Request` as ASP.NET Core `ValidationProblemDetails` before application logic runs.

**Independent Test**: Submit missing body, blank `title`, missing/out-of-range `releaseYear`, invalid `countryCode`, blank `originalLanguage`, blank genre entries, and non-positive `durationMinutes`; expect field-level validation errors and no persisted movie.

### Tests for User Story 2

- [X] T030 [P] [US2] Add validator tests for required fields, `1888` through next-calendar-year release range, two-letter country code, blank language, blank genres, and positive duration in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieRequestValidatorTests.cs`
- [X] T031 [US2] Add API tests for missing body and validation failures returning `ValidationProblemDetails` in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointValidationTests.cs`
- [X] T032 [P] [US2] Add domain tests for invalid movie invariants that must not be bypassed by Application code in `backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs`

### Implementation for User Story 2

- [X] T033 [US2] Add `CreateMovieRequestValidator` with OpenAPI-aligned validation rules in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieRequestValidator.cs`
- [X] T034 [US2] Register `IValidator<CreateMovieRequest>` in `backend/src/SmartMovieCatalog.Api/Program.cs`
- [X] T035 [US2] Add `ValidationFilter<CreateMovieRequest>` and validation response metadata to `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`
- [X] T036 [US2] Ensure expected create movie failures map to safe `ValidationProblemDetails` or `ProblemDetails` without internal details in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`

**Checkpoint**: User Story 2 is functional and does not introduce authentication, frontend behavior, or external dependencies.

---

## Phase 5: User Story 3 - Maintainable Contracts, Persistence, And Documentation (Priority: P3)

**Goal**: The implementation is reviewable through explicit contracts, Clean Architecture boundaries, normalized persistence mapping, focused tests, quickstart checks, and updated docs.

**Independent Test**: Review source paths and docs to confirm contracts do not expose persistence models, business logic is outside the API handler, `GET /api/movies/{id}` is not added, normalized genre persistence is documented, and backend tests/builds pass.

### Tests for User Story 3

- [X] T037 [P] [US3] Add contract-shape assertions for `MovieResponse` JSON serialization and nullable optional fields in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs`
- [X] T038 [US3] Add API contract test that `POST /api/movies` returns `Location: /api/movies/{id}` without adding a `GET /api/movies/{id}` endpoint in `backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieEndpointContractTests.cs`
- [X] T039 [US3] Run focused backend test projects referenced by `specs/011-create-movie/quickstart.md` using `backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj`, `backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj`, `backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj`, and `backend/tests/SmartMovieCatalog.Infrastructure.Tests/SmartMovieCatalog.Infrastructure.Tests.csproj`

### Implementation for User Story 3

- [X] T040 [US3] Update movie endpoint documentation in `docs/API.md`
- [X] T041 [US3] Update initial movie domain decisions for identity, uniqueness, trust, ownership, and AI provenance in `docs/DOMAIN.md`
- [X] T042 [US3] Update architecture current state for movie persistence only if implemented boundaries change documented behavior in `docs/ARCHITECTURE.md`
- [X] T043 [US3] Add or update manual HTTP example for `POST /api/movies` in `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.http`
- [X] T044 [US3] Verify public DTOs do not reference Domain, Infrastructure, EF Core, or provider-specific types in `backend/src/SmartMovieCatalog.Contracts/Movies/`

**Checkpoint**: User Story 3 makes the slice maintainable and reviewable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final verification, cleanup, and safety checks across the create movie slice.

- [X] T045 [P] Review naming and nullable annotations across `backend/src/SmartMovieCatalog.Contracts/Movies/`, `backend/src/SmartMovieCatalog.Application/Features/Movies/`, `backend/src/SmartMovieCatalog.Domain/Movies/`, and `backend/src/SmartMovieCatalog.Infrastructure/Persistence/`
- [X] T046 [P] Review logging and error handling to avoid raw request bodies, synopsis text, EF Core details, connection strings, and internal exception details in `backend/src/SmartMovieCatalog.Api/Features/Movies/CreateMovie/CreateMovieEndpoint.cs`
- [X] T047 Verify quickstart valid and invalid payload scenarios from `specs/011-create-movie/quickstart.md`
- [X] T048 Run `dotnet build SmartMovieCatalog.slnx` against `SmartMovieCatalog.slnx`
- [X] T049 Review `git diff` for unrelated changes across `backend/src/`, `backend/tests/`, `docs/`, and `specs/011-create-movie/tasks.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 Setup**: No dependencies.
- **Phase 2 Foundational**: Depends on Phase 1 and blocks all user stories.
- **Phase 3 US1**: Depends on Phase 2 and delivers the MVP.
- **Phase 4 US2**: Depends on Phase 2 and integrates with the endpoint from US1.
- **Phase 5 US3**: Depends on the implemented contract/API/domain/persistence shape from US1 and US2.
- **Phase 6 Polish**: Depends on selected user stories being complete.

### User Story Dependencies

- **US1 Create Movie Successfully**: MVP; can start after Phase 2.
- **US2 Validate Invalid Movie Input**: Can start after Phase 2; endpoint integration is simplest after T027 creates the endpoint.
- **US3 Maintainable Contracts, Persistence, And Documentation**: Depends on final implementation shape from US1 and US2.

### Within Each User Story

- Tests should be written first and fail before implementation where practical.
- Domain and contract models precede Application handlers.
- Application handlers precede API endpoint wiring.
- EF Core mapping and repository registration precede persistence-backed endpoint success.
- Documentation follows the implemented public behavior and durable domain decisions.

## Parallel Opportunities

- T006, T007, T008, and T009 can run in parallel after setup.
- T016, T017, T019, and T022 can run in parallel once foundational files exist.
- T030 and T032 can run in parallel for validation behavior.
- T040, T041, and T042 can run in parallel after implementation behavior is stable.
- T045 and T046 can run in parallel during final polish.

## Parallel Example: User Story 1

```text
Task: "T016 [P] [US1] Add domain tests for valid movie creation, GUID identity, field trimming, country uppercase normalization, and optional metadata in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieTests.cs"
Task: "T017 [P] [US1] Add application fake movie repository in backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeMovieRepository.cs"
Task: "T019 [P] [US1] Add API fake movie repository support in backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/FakeMovieRepository.cs"
Task: "T022 [US1] Add infrastructure persistence test for saving and reading a movie with normalized genres in backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfMovieRepositoryTests.cs"
```

## Parallel Example: User Story 2

```text
Task: "T030 [P] [US2] Add validator tests for required fields, 1888 through next-calendar-year release range, two-letter country code, blank language, blank genres, and positive duration in backend/tests/SmartMovieCatalog.Api.Tests/Movies/CreateMovieRequestValidatorTests.cs"
Task: "T032 [P] [US2] Add domain tests for invalid movie invariants that must not be bypassed by Application code in backend/tests/SmartMovieCatalog.Domain.Tests/Movies/MovieValidationTests.cs"
```

## Implementation Strategy

### MVP First

1. Complete Phase 1.
2. Complete Phase 2.
3. Complete Phase 3 for US1.
4. Validate that `POST /api/movies` returns `201 Created`, a GUID `id`, normalized `countryCode`, and `Location: /api/movies/{id}` for the quickstart payload.
5. Stop and review before expanding validation and documentation if a smaller review slice is desired.

### Incremental Delivery

1. Deliver US1 to create and persist movies.
2. Deliver US2 to harden invalid-input behavior.
3. Deliver US3 to finalize documentation, contract checks, and architecture reviewability.
4. Complete polish and full solution build.

### Verification Commands

```powershell
dotnet test backend/tests/SmartMovieCatalog.Domain.Tests/SmartMovieCatalog.Domain.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj
dotnet test backend/tests/SmartMovieCatalog.Infrastructure.Tests/SmartMovieCatalog.Infrastructure.Tests.csproj
dotnet build SmartMovieCatalog.slnx
```

## Notes

- Do not add frontend code unless an existing movie creation page is discovered and explicitly pulled into scope.
- Do not introduce authentication, authorization, AI, SignalR, semantic search, RAG, TMDb, bulk import, duplicate detection, distributed messaging, Wolverine transports, background workers, or new persistence providers.
- Existing Wolverine in-process dispatch is allowed only as the current Application mediator pattern; do not add new Wolverine architecture.
- Keep generated/vendor paths out of scope: `node_modules/`, `bin/`, `obj/`, `dist/`, `.vs/`, `.angular/cache/`.
