# Tasks: Basic Title Search

**Input**: Design documents from `specs/016-basic-title-search/`  
**Prerequisites**: `plan.md` (required), `spec.md` (required)

**Tests**: Included because the specification and plan define explicit acceptance and verification behavior for backend and frontend.

**Organization**: Tasks are grouped by user story to keep each increment independently testable.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Task can run in parallel (separate files, no dependency on incomplete tasks)
- **[Story]**: User story label for story phases (`US1`, `US2`, `US3`)
- Every task includes an exact file path

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Confirm contract baseline and feature boundaries before story implementation.

- [x] T001 Review and align API search contract wording in `eng/docs/API.md` for `GET /api/movies?query=&page=&pageSize=`.
- [x] T002 Review and align frontend catalog/search expectations in `eng/docs/FRONTEND.md` with basic title search wording.
- [x] T003 [P] Confirm list endpoint mapping registration for search flow in `backend/src/SmartMovieCatalog.Api/Features/Movies/MoviesEndpoints.cs`.
- [x] T004 [P] Confirm typed API boundary usage for movie listing in `frontend/src/app/movies/movies-api.ts`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Ensure shared backend/frontend contract and validation primitives are stable.

**CRITICAL**: User story work begins only after this phase.

- [x] T005 Verify `ListMoviesRequest` defaults (`page=1`, `pageSize=12`) in `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesRequest.cs`.
- [x] T006 Verify request validation bounds for paging in `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesRequestValidator.cs`.
- [x] T007 Verify endpoint query normalization and `PagedMovieSummaryResponse` projection in `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesEndpoint.cs`.
- [x] T008 Verify repository filter hook for title/original-title query path in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`.
- [x] T009 Verify shared frontend response contract fields in `frontend/src/app/movies/movie.models.ts`.
- [x] T010 Verify query parameter serialization/blank-query omission in `frontend/src/app/movies/movies-api.ts`.

**Checkpoint**: API boundary, validation, and typed model contract are ready for user stories.

---

## Phase 3: User Story 1 - Search Movies by Title (Priority: P1) 🎯 MVP

**Goal**: Users can search movies by title using the existing list endpoint with case-insensitive behavior and stable response shape.

**Independent Test**: Query existing movies by title/original title and verify filtered results are returned without changing response schema.

### Tests for User Story 1

- [x] T011 [P] [US1] Add endpoint-level test for optional `query` behavior in `backend/tests/SmartMovieCatalog.Api.Tests/Features/Movies/ListMoviesEndpointTests.cs`.
- [x] T012 [P] [US1] Add repository test for title and original-title matching in `backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfMovieRepositoryTests.cs`.
- [x] T013 [P] [US1] Add repository test for case-insensitive search semantics in `backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfMovieRepositoryTests.cs`.
- [x] T014 [P] [US1] Add API client test for `query` serialization in `frontend/src/app/movies/movies-api.spec.ts`.

### Implementation for User Story 1

- [x] T015 [US1] Implement/adjust query normalization in `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesEndpoint.cs` to trim and nullify blank values.
- [x] T016 [US1] Implement/adjust repository filter over `Title` and `OriginalTitle` in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`.
- [x] T017 [US1] Ensure application mapping keeps public response stable in `backend/src/SmartMovieCatalog.Application/Features/Movies/ListMoviesHandler.cs`.
- [x] T018 [US1] Ensure frontend API service sends optional query only when non-blank in `frontend/src/app/movies/movies-api.ts`.
- [x] T019 [US1] Ensure catalog search form submission uses typed `MoviesApi` list query in `frontend/src/app/catalog/catalog-page/catalog-page.ts`.

**Checkpoint**: Title search works end-to-end and response contract remains unchanged.

---

## Phase 4: User Story 2 - Search with Pagination and Invalid Input Handling (Priority: P2)

**Goal**: Search cooperates with pagination and invalid parameters return `400` problem responses.

**Independent Test**: Search with page navigation and invalid paging inputs; verify valid paged search responses and consistent `400` behavior for invalid requests.

### Tests for User Story 2

- [x] T020 [P] [US2] Add API test for combined `query + page + pageSize` response metadata in `backend/tests/SmartMovieCatalog.Api.Tests/Features/Movies/ListMoviesEndpointTests.cs`.
- [x] T021 [P] [US2] Add API test for invalid paging returning `400` validation/problem response in `backend/tests/SmartMovieCatalog.Api.Tests/Features/Movies/ListMoviesEndpointTests.cs`.
- [x] T022 [P] [US2] Add frontend `MoviesApi` test for page and pageSize serialization with query in `frontend/src/app/movies/movies-api.spec.ts`.
- [x] T023 [P] [US2] Add catalog page test for pagination navigation with active query state in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`.

### Implementation for User Story 2

- [x] T024 [US2] Ensure endpoint validation and response metadata mapping are consistent in `backend/src/SmartMovieCatalog.Api/Features/Movies/ListMovies/ListMoviesEndpoint.cs`.
- [x] T025 [US2] Ensure repository paging is applied after filtering in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`.
- [x] T026 [US2] Ensure catalog URL state keeps `query`, `page`, and `pageSize` synchronized in `frontend/src/app/catalog/catalog-page/catalog-page.ts`.
- [x] T027 [US2] Ensure pagination controls preserve active query context in `frontend/src/app/catalog/catalog-page/catalog-page.html`.

**Checkpoint**: Search + pagination is stable and invalid input behavior is explicit and test-covered.

---

## Phase 5: User Story 3 - Clear Search States in Catalog UI (Priority: P3)

**Goal**: Users see clear loading, empty-catalog, no-result, and error states during search.

**Independent Test**: Simulate loading, empty default list, empty filtered result, and API failure; verify each state renders distinct, predictable UI.

### Tests for User Story 3

- [x] T028 [P] [US3] Add catalog test for empty-catalog state when query is blank in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`.
- [x] T029 [P] [US3] Add catalog test for no-result state when query is non-blank in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`.
- [x] T030 [P] [US3] Add catalog test for generic API error state during search in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`.

### Implementation for User Story 3

- [x] T031 [US3] Ensure `loading/success/emptyCatalog/noResults/error` transitions are consistent in `frontend/src/app/catalog/catalog-page/catalog-page.ts`.
- [x] T032 [US3] Ensure distinct state blocks/messages are rendered in `frontend/src/app/catalog/catalog-page/catalog-page.html`.
- [x] T033 [US3] Ensure state styling and visibility remain stable across viewport sizes in `frontend/src/app/catalog/catalog-page/catalog-page.css`.

**Checkpoint**: Search state UX is independently verifiable and consistent with specification.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency checks, documentation sync, and full verification run.

- [x] T034 Update feature notes and any contract wording drift in `specs/016-basic-title-search/spec.md` and `specs/016-basic-title-search/plan.md` if implementation decisions changed.
- [x] T035 [P] Run backend API build validation with `dotnet build backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`.
- [x] T036 [P] Run solution-level build validation with `dotnet build SmartMovieCatalog.slnx`.
- [x] T037 [P] Run frontend tests with `npm test -- --watch=false` from `frontend/package.json`.
- [x] T038 [P] Run frontend build with `npm run build` from `frontend/package.json`.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Phase 1 (Setup)**: No dependencies.
- **Phase 2 (Foundational)**: Depends on Phase 1 and blocks story work.
- **Phase 3 (US1)**: Depends on Phase 2; MVP.
- **Phase 4 (US2)**: Depends on Phase 3 filtering baseline.
- **Phase 5 (US3)**: Depends on Phase 3/4 search pipeline for realistic UI states.
- **Phase 6 (Polish)**: Depends on completed story phases.

### User Story Dependencies

- **US1 (P1)**: Independent after foundational tasks.
- **US2 (P2)**: Builds on US1 query/filter behavior.
- **US3 (P3)**: Uses search pipeline from US1/US2 for state handling.

### Within Each User Story

- Tests should be written first and fail before implementation.
- Backend normalization/filter behavior before frontend orchestration adjustments.
- Component/state logic before template and CSS polishing.
- Complete story checkpoint validation before moving to the next priority.

---

## Parallel Opportunities

- **Setup**: `T003` and `T004` can run in parallel.
- **Foundational**: `T009` and `T010` can run in parallel with backend checks `T005`-`T008`.
- **US1 Tests**: `T011`-`T014` can run in parallel.
- **US2 Tests**: `T020`-`T023` can run in parallel.
- **US3 Tests**: `T028`-`T030` can run in parallel.
- **Polish Verification**: `T035`-`T038` can run in parallel when environment capacity allows.

---

## Parallel Example: User Story 1

```text
Task: "T011 [US1] Add endpoint optional query test in backend/tests/SmartMovieCatalog.Api.Tests/Features/Movies/ListMoviesEndpointTests.cs"
Task: "T013 [US1] Add case-insensitive repository test in backend/tests/SmartMovieCatalog.Infrastructure.Tests/Persistence/EfMovieRepositoryTests.cs"
Task: "T014 [US1] Add query serialization test in frontend/src/app/movies/movies-api.spec.ts"
```

## Parallel Example: User Story 2

```text
Task: "T020 [US2] Add query+pagination API response test in backend/tests/SmartMovieCatalog.Api.Tests/Features/Movies/ListMoviesEndpointTests.cs"
Task: "T022 [US2] Add MoviesApi query/page/pageSize serialization test in frontend/src/app/movies/movies-api.spec.ts"
Task: "T023 [US2] Add catalog pagination-with-query test in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
```

## Parallel Example: User Story 3

```text
Task: "T028 [US3] Add empty-catalog state test in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T029 [US3] Add no-results state test in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T030 [US3] Add error-state test in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
```

---

## Implementation Strategy

### MVP First (US1 only)

1. Finish Phase 1 and Phase 2.
2. Deliver Phase 3 (US1) with end-to-end query filtering.
3. Validate US1 independently before expanding scope.

### Incremental Delivery

1. Deliver US1 for basic title search.
2. Deliver US2 for search + pagination + invalid-input handling.
3. Deliver US3 for complete UI state handling.
4. Run Phase 6 cross-cutting validation and documentation sync.

### Parallel Team Strategy

1. Complete setup/foundational tasks together.
2. Split story tasks by concern after foundation:
   - Backend filtering/validation owner
   - Frontend API/catalo-state owner
   - Test coverage owner
3. Merge by story checkpoints to avoid cross-story regressions.

---

## Notes

- `[P]` marks tasks safe for parallelization with disjoint file ownership.
- Story labels provide traceability from tasks to `spec.md` user stories.
- Preserve explicit non-goals: no semantic/vector/AI search behavior.
- Keep components free of direct `HttpClient`; use typed `MoviesApi`.
