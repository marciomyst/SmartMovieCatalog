# Tasks: Catalog Page

**Input**: Design documents from `specs/018-catalog-page/`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/catalog-ui.md`, `quickstart.md`
**Tests**: Included because `plan.md` requires focused frontend API service and catalog page tests.
**Scope**: Frontend implementation for issue #18. Backend list/search/details endpoints from issues #12, #16, and #13 remain prerequisites unless scope is explicitly expanded.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files or has no dependency on incomplete tasks.
- **[Story]**: Maps to the user stories in `spec.md`.
- Every task includes an exact repository path.

---

## Phase 1: Setup (Shared Context)

**Purpose**: Confirm the refreshed issue #18 planning context and current frontend structure before implementation.

- [X] T001 Review clarified decisions in `specs/018-catalog-page/spec.md`
- [X] T002 [P] Review URL/query and public-route decisions in `specs/018-catalog-page/research.md`
- [X] T003 [P] Review frontend model definitions in `specs/018-catalog-page/data-model.md`
- [X] T004 [P] Review UI/API contract requirements in `specs/018-catalog-page/contracts/catalog-ui.md`
- [X] T005 [P] Review manual verification expectations in `specs/018-catalog-page/quickstart.md`
- [X] T006 Inspect current Angular bootstrap and router setup in `frontend/src/main.ts`
- [X] T007 [P] Inspect current root app shell files in `frontend/src/app/app.ts`, `frontend/src/app/app.html`, and `frontend/src/app/app.css`
- [X] T008 [P] Inspect existing frontend HTTP test pattern in `frontend/src/app/auth/auth-api.spec.ts`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Add shared movie API models and service infrastructure needed by every catalog user story.

**Critical**: No catalog page story can be completed until this phase is complete.

- [X] T009 Create `MovieSummary`, `PagedMovieSummaryResponse`, `MovieListQuery`, `CatalogViewState`, and `CatalogLoadState` in `frontend/src/app/movies/movie.models.ts`
- [X] T010 Implement typed `MoviesApi.listMovies()` for `GET /api/movies` in `frontend/src/app/movies/movies-api.ts`
- [X] T011 [P] Add `MoviesApi` test for `page`, `pageSize`, and non-empty `query` parameters in `frontend/src/app/movies/movies-api.spec.ts`
- [X] T012 [P] Add `MoviesApi` test that blank `query` is omitted from the request in `frontend/src/app/movies/movies-api.spec.ts`
- [X] T013 [P] Add `MoviesApi` test for the expected paginated response shape in `frontend/src/app/movies/movies-api.spec.ts`

**Checkpoint**: Typed API access exists, and catalog components can consume movie summaries without direct `HttpClient` usage.

---

## Phase 3: User Story 1 - Open Dedicated Catalog Page (Priority: P1) MVP

**Goal**: A user can navigate to public `/catalog` and see a dedicated catalog browsing surface.

**Independent Test**: Navigate to `/catalog` without an authenticated frontend session and verify the Catalog page renders with the app navigation.

### Tests for User Story 1

- [X] T014 [P] [US1] Add app-shell test for a Catalog navigation link in `frontend/src/app/app.spec.ts`
- [X] T015 [P] [US1] Add route test proving `/catalog` renders without an auth guard in `frontend/src/app/app.spec.ts`
- [X] T016 [P] [US1] Add initial catalog page render test in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`

### Implementation for User Story 1

- [X] T017 [US1] Register the public `/catalog` route using Angular router providers in `frontend/src/main.ts`
- [X] T018 [US1] Update root app imports and router outlet support in `frontend/src/app/app.ts`
- [X] T019 [US1] Add fixed top navigation with a Catalog link and router outlet in `frontend/src/app/app.html`
- [X] T020 [US1] Style the app shell and active Catalog navigation state using `frontend/DESIGN.md` tokens in `frontend/src/app/app.css`
- [X] T021 [US1] Create the catalog page component class with initial load state in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T022 [US1] Create base catalog page markup with title, search area, result area, and pagination area in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T023 [US1] Style the base catalog layout responsively in `frontend/src/app/catalog/catalog-page/catalog-page.css`

**Checkpoint**: `/catalog` is publicly reachable through app navigation and renders a responsive page skeleton.

---

## Phase 4: User Story 2 - Browse Movies From API (Priority: P2)

**Goal**: A user can browse movie summaries loaded from the API.

**Independent Test**: Mock `MoviesApi` with movie summaries and verify the catalog page renders titles and metadata without direct `HttpClient` usage.

### Tests for User Story 2

- [X] T024 [P] [US2] Add catalog success-state test with mocked movie summaries in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`
- [X] T025 [P] [US2] Add test proving catalog page uses `MoviesApi` and not component `HttpClient` setup in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`

### Implementation for User Story 2

- [X] T026 [US2] Inject and call `MoviesApi` from the catalog page in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T027 [US2] Render movie summary cards or list items from API data in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T028 [US2] Add responsive movie grid/list styles aligned with `frontend/DESIGN.md` in `frontend/src/app/catalog/catalog-page/catalog-page.css`
- [X] T029 [US2] Reuse the issue #17 movie card if present, otherwise keep catalog-local card markup in `frontend/src/app/catalog/catalog-page/catalog-page.html`

**Checkpoint**: The catalog page lists movies from the typed API service and remains testable with mocked data.

---

## Phase 5: User Story 3 - Search Movies By Title With URL State (Priority: P3)

**Goal**: A user can search movies by title and share/refresh the resulting catalog URL.

**Independent Test**: Open `/catalog?query=central&page=2&pageSize=12`, verify the page reads URL state, then search for a new query and verify the URL updates with `page=1`.

### Tests for User Story 3

- [X] T030 [P] [US3] Add test that catalog reads initial `query`, `page`, and `pageSize` from URL query parameters in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`
- [X] T031 [P] [US3] Add search test that updates URL query parameters and resets `page` to `1` in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`
- [X] T032 [P] [US3] Add test that blank search omits `query` from API request and URL state in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`

### Implementation for User Story 3

- [X] T033 [US3] Parse `query`, `page`, and `pageSize` from `ActivatedRoute` query parameters in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T034 [US3] Add search form state and submission handling in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T035 [US3] Synchronize search changes to router query parameters in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T036 [US3] Wire the search input and submit action to catalog search in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T037 [US3] Style the search input and action state according to `frontend/DESIGN.md` in `frontend/src/app/catalog/catalog-page/catalog-page.css`

**Checkpoint**: Basic title search is URL-backed, shareable, refresh-safe, and free of semantic/AI search terminology.

---

## Phase 6: User Story 4 - Use Explicit Pagination And Catalog States (Priority: P4)

**Goal**: A user sees clear loading, empty, no-result, error, and pagination states driven by API metadata.

**Independent Test**: Mock loading, empty catalog, no-result, API failure, previous-page, and next-page responses and verify visible states, disabled controls, and URL updates.

### Tests for User Story 4

- [X] T038 [P] [US4] Add catalog loading, empty catalog, no-result, and error state tests in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`
- [X] T039 [P] [US4] Add pagination tests for `hasPreviousPage`, `hasNextPage`, and URL updates in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`
- [X] T040 [P] [US4] Add invalid URL `page` and `pageSize` normalization tests in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`

### Implementation for User Story 4

- [X] T041 [US4] Implement `loading`, `success`, `emptyCatalog`, `noResults`, and `error` load state derivation in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T042 [US4] Normalize absent or invalid URL `page` to `1` and `pageSize` to `12` before API calls in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T043 [US4] Implement previous and next pagination handlers that update router query parameters in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T044 [US4] Render loading, empty catalog, no-result, and generic API error states in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T045 [US4] Render pagination controls using `page`, `pageSize`, `totalCount`, `totalPages`, `hasPreviousPage`, and `hasNextPage` in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T046 [US4] Ensure no page-size selector is rendered in V1 in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T047 [US4] Style catalog states and pagination controls responsively in `frontend/src/app/catalog/catalog-page/catalog-page.css`

**Checkpoint**: Catalog state handling, invalid URL normalization, and explicit pagination are complete and testable.

---

## Phase 7: User Story 5 - Navigate To Movie Details (Priority: P5)

**Goal**: A user can open a movie details page from a catalog item.

**Independent Test**: Render a movie item and verify its link targets `/movies/{id}` unless issue #13 establishes a different route first.

### Tests for User Story 5

- [X] T048 [P] [US5] Add catalog item details-link test for `/movies/{id}` in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`

### Implementation for User Story 5

- [X] T049 [US5] Add details route link generation for each catalog item in `frontend/src/app/catalog/catalog-page/catalog-page.ts`
- [X] T050 [US5] Apply details navigation links to catalog item markup in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T051 [US5] Add accessible focus and hover styles for catalog item navigation in `frontend/src/app/catalog/catalog-page/catalog-page.css`

**Checkpoint**: Each catalog item navigates to the movie details route without adding details-page behavior to issue #18.

---

## Phase 8: Polish & Cross-Cutting Concerns

**Purpose**: Validate the complete frontend slice against the refreshed plan, quickstart, and acceptance criteria.

- [X] T052 [P] Check visible catalog UI copy for unsupported terms in `frontend/src/app/catalog/catalog-page/catalog-page.html`
- [X] T053 [P] Verify manual scenarios from `specs/018-catalog-page/quickstart.md`
- [X] T054 [P] Update frontend documentation if routing or API consumption structure changed in `docs/FRONTEND.md`
- [X] T055 Run frontend unit tests with `npm test -- --watch=false` from `frontend/package.json`
- [X] T056 Run frontend build with `npm run build` from `frontend/package.json`
- [X] T057 Review implementation against issue #18 acceptance criteria in `specs/018-catalog-page/spec.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Setup and blocks all user stories.
- **US1 Dedicated Catalog Route (Phase 3)**: Depends on Foundational.
- **US2 API-Backed Browsing (Phase 4)**: Depends on Foundational and works best after US1 route shell.
- **US3 URL-Backed Search (Phase 5)**: Depends on US2 API loading and US1 routing.
- **US4 States And Pagination (Phase 6)**: Depends on US3 URL state and US2 response handling.
- **US5 Details Navigation (Phase 7)**: Depends on US2 rendered catalog items and issue #13 route availability.
- **Polish (Phase 8)**: Depends on all selected stories.

### User Story Dependencies

- **US1 (P1)**: Can start after Foundational and provides the MVP route.
- **US2 (P2)**: Can start after Foundational; needs the typed `MoviesApi`.
- **US3 (P3)**: Depends on catalog route and API loading.
- **US4 (P4)**: Depends on URL state and API response metadata.
- **US5 (P5)**: Depends on rendered movie items and the details route contract.

### Parallel Opportunities

- T002, T003, T004, T005, T007, and T008 can run in parallel during setup.
- T011, T012, and T013 can run in parallel after `MoviesApi` shape is defined.
- Tests marked `[P]` within each user story can be written in parallel before implementation.
- CSS tasks can usually proceed after related markup exists while TypeScript state work continues.

---

## Parallel Example: User Story 3

```text
Task: "T030 [P] [US3] Add test that catalog reads initial query, page, and pageSize from URL query parameters in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T031 [P] [US3] Add search test that updates URL query parameters and resets page to 1 in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T032 [P] [US3] Add test that blank search omits query from API request and URL state in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
```

---

## Parallel Example: User Story 4

```text
Task: "T038 [P] [US4] Add catalog loading, empty catalog, no-result, and error state tests in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T039 [P] [US4] Add pagination tests for hasPreviousPage, hasNextPage, and URL updates in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
Task: "T040 [P] [US4] Add invalid URL page and pageSize normalization tests in frontend/src/app/catalog/catalog-page/catalog-page.spec.ts"
```

---

## Implementation Strategy

### MVP First

1. Complete Phase 1 and Phase 2.
2. Complete US1 to make public `/catalog` reachable.
3. Complete US2 to list API-backed movies.
4. Stop and validate with mocked API data before adding URL-backed search, full state handling, pagination, and details navigation.

### Incremental Delivery

1. Route shell and public navigation.
2. Typed API service and API-backed browsing.
3. URL-backed basic title search.
4. Loading/empty/no-result/error states and pagination.
5. Details navigation.
6. Tests, build, quickstart verification, and documentation check.

### Validation

- Run `npm test -- --watch=false` from `frontend`.
- Run `npm run build` from `frontend`.
- If prerequisite API contracts are changed outside issue #18, run the relevant backend build from `SmartMovieCatalog.slnx` or `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`.

---

## Notes

- Do not implement backend list/search/details endpoints inside issue #18 unless scope is explicitly expanded.
- Do not add route guards or require authentication for `/catalog` in V1.
- Do not render a page-size selector in V1.
- Components must consume `MoviesApi`; they must not inject `HttpClient` directly.
- Keep search as basic title search only.
- Keep UI copy free of unsupported V1 terms such as RAG, Qdrant, vector search, semantic search, CLIP, agent framework, recommendations, and AI ranking.
