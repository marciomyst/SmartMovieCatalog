# Tasks: Home Movie Cards

**Input**: Design documents from `specs/017-home-movie-cards/`  
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/home-ui.md`, `quickstart.md`

**Tests**: Included because the specification defines independent tests for each user story and the frontend already has component/API test structure.

**Organization**: Tasks are grouped by user story so each story can be implemented and validated independently.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (US1, US2, US3)
- Each task includes exact file paths

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Establish route ownership and documentation prerequisites for the feature.

- [X] T001 Update routing in `frontend/src/main.ts` so `/` maps to the new home page and `/login` maps to the existing login page.
- [X] T002 Update app navigation labels/links in `frontend/src/app/app.html` for Home, Catalog, and Login routes.
- [X] T003 [P] Update frontend routing documentation in `docs/FRONTEND.md` to describe `/`, `/login`, `/catalog`, and `/movies/:id`.
- [X] T004 [P] Update shell route/navigation expectations in `frontend/src/app/app.spec.ts`.

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Create the home feature shell that all user stories build on.

**CRITICAL**: No user story work should begin until this phase is complete.

- [X] T005 Create `frontend/src/app/home/home-page/home-page.ts` with a standalone home page component shell and imports needed for template rendering.
- [X] T006 Create `frontend/src/app/home/home-page/home-page.html` with semantic page structure and an empty movie section placeholder.
- [X] T007 Create `frontend/src/app/home/home-page/home-page.css` aligned with `frontend/DESIGN.md` dark cinematic tokens.
- [X] T008 Create `frontend/src/app/home/home-page/home-page.spec.ts` with test setup and mocked `MoviesApi` boundary.

**Checkpoint**: Home route exists and can render an empty shell without movie data.

---

## Phase 3: User Story 1 - Browse Movies From Home (Priority: P1) MVP

**Goal**: Visitors opening `/` see up to 6 real movie cards from catalog data.

**Independent Test**: Mock catalog data, open the home page, and verify 6 cards render when at least 6 movies exist, fewer cards render when fewer movies exist, and card content uses real movie summary data.

### Tests for User Story 1

- [X] T009 [P] [US1] Add home page success-state tests in `frontend/src/app/home/home-page/home-page.spec.ts` for rendering exactly 6 cards when at least 6 movies are returned.
- [X] T010 [P] [US1] Add home page reduced-result tests in `frontend/src/app/home/home-page/home-page.spec.ts` for rendering fewer than 6 cards when fewer movies are returned.
- [X] T011 [P] [US1] Add poster image and poster placeholder tests in `frontend/src/app/home/home-page/home-page.spec.ts`.
- [X] T012 [P] [US1] Add prohibited-terminology assertion in `frontend/src/app/home/home-page/home-page.spec.ts` for home page rendered text.

### Implementation for User Story 1

- [X] T013 [US1] Inject and call `MoviesApi.listMovies({ page: 1, pageSize: 6 })` in `frontend/src/app/home/home-page/home-page.ts`.
- [X] T014 [US1] Add home movie list state and success mapping in `frontend/src/app/home/home-page/home-page.ts`.
- [X] T015 [US1] Render movie cards with title, release year, country/basic metadata, director, genres, and poster/placeholder in `frontend/src/app/home/home-page/home-page.html`.
- [X] T016 [US1] Style the home movie grid and card visuals in `frontend/src/app/home/home-page/home-page.css`.
- [X] T017 [US1] Add or reuse poster URL normalization in `frontend/src/app/home/home-page/home-page.ts` without introducing direct `HttpClient` usage.

**Checkpoint**: User Story 1 is functional and independently testable at `/`.

---

## Phase 4: User Story 2 - Understand Home Page States (Priority: P2)

**Goal**: Visitors see clear loading, empty, and error states for the home movie section.

**Independent Test**: Mock pending, empty, and failed catalog responses, then verify each state renders a distinct message and no stale cards.

### Tests for User Story 2

- [X] T018 [P] [US2] Add loading-state test in `frontend/src/app/home/home-page/home-page.spec.ts`.
- [X] T019 [P] [US2] Add empty-state test in `frontend/src/app/home/home-page/home-page.spec.ts`.
- [X] T020 [P] [US2] Add error-state test in `frontend/src/app/home/home-page/home-page.spec.ts`.

### Implementation for User Story 2

- [X] T021 [US2] Add `loading`, `success`, `empty`, and `error` load states in `frontend/src/app/home/home-page/home-page.ts`.
- [X] T022 [US2] Render loading, empty, and error state blocks in `frontend/src/app/home/home-page/home-page.html`.
- [X] T023 [US2] Style state blocks in `frontend/src/app/home/home-page/home-page.css` with accessible contrast and no layout shift.
- [X] T024 [US2] Ensure failed requests show generic error copy and do not render stale cards in `frontend/src/app/home/home-page/home-page.ts`.

**Checkpoint**: User Story 2 works independently with mocked API states.

---

## Phase 5: User Story 3 - Open Movie Details From Home (Priority: P3)

**Goal**: Every rendered home movie card navigates to the matching movie details page.

**Independent Test**: Render home cards with movie IDs, activate a card, and verify the generated route target is `/movies/{id}`.

### Tests for User Story 3

- [X] T025 [P] [US3] Add details-link target tests in `frontend/src/app/home/home-page/home-page.spec.ts`.
- [X] T026 [P] [US3] Add keyboard/pointer accessible card semantics test in `frontend/src/app/home/home-page/home-page.spec.ts`.

### Implementation for User Story 3

- [X] T027 [US3] Add `detailsLink(movie)` route target helper in `frontend/src/app/home/home-page/home-page.ts`.
- [X] T028 [US3] Wrap each rendered home card with router navigation to `/movies/{id}` in `frontend/src/app/home/home-page/home-page.html`.
- [X] T029 [US3] Add accessible card labels and focus-visible styling in `frontend/src/app/home/home-page/home-page.html` and `frontend/src/app/home/home-page/home-page.css`.

**Checkpoint**: User Story 3 works independently and all home cards navigate to details.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Reduce duplication, validate contracts, and complete feature verification.

- [X] T030 Evaluate catalog/home card duplication and decide whether to extract shared card behavior; document decision in `specs/017-home-movie-cards/plan.md` if implementation differs from the conditional plan.
- [X] T031 [P] If extraction is chosen, create shared card component files under `frontend/src/app/shared/movie-card/`. (N/A: extraction not selected in T030)
- [X] T032 If extraction is chosen, update `frontend/src/app/catalog/catalog-page/catalog-page.html` and `frontend/src/app/catalog/catalog-page/catalog-page.ts` to use the shared card without changing catalog behavior. (N/A: extraction not selected in T030)
- [X] T033 If extraction is chosen, update catalog card tests in `frontend/src/app/catalog/catalog-page/catalog-page.spec.ts`. (N/A: extraction not selected in T030)
- [X] T034 [P] Run quickstart validation steps from `specs/017-home-movie-cards/quickstart.md`.
- [X] T035 Run frontend tests with `npm test -- --watch=false` from `frontend`.
- [X] T036 Run frontend build with `npm run build` from `frontend`.

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories.
- **User Story 1 (Phase 3)**: Depends on Foundational phase; MVP scope.
- **User Story 2 (Phase 4)**: Depends on Foundational phase and can be tested with mocked data, but integrates most naturally after US1 state wiring.
- **User Story 3 (Phase 5)**: Depends on card rendering from US1.
- **Polish (Phase 6)**: Depends on selected user stories being complete.

### User Story Dependencies

- **US1 Browse Movies From Home**: MVP; no dependency on US2 or US3.
- **US2 Understand Home Page States**: Can be developed after foundation, but final integration uses the loading pipeline introduced by US1.
- **US3 Open Movie Details From Home**: Depends on rendered cards from US1.

### Within Each User Story

- Tests should be written before implementation.
- Component state before template integration.
- Template integration before CSS polish.
- Story checkpoint should pass before moving to next priority.

## Parallel Opportunities

- T003 and T004 can run in parallel with route setup.
- T005, T006, T007, and T008 touch different new files and can be split after route imports are known.
- T009 through T012 can be written in parallel before US1 implementation.
- T018 through T020 can be written in parallel before US2 implementation.
- T025 and T026 can be written in parallel before US3 implementation.
- T031 can run in parallel with quickstart review if card extraction is selected and catalog files are not being edited simultaneously.

## Parallel Example: User Story 1

```text
Task: "Add home page success-state tests in frontend/src/app/home/home-page/home-page.spec.ts"
Task: "Add poster image and poster placeholder tests in frontend/src/app/home/home-page/home-page.spec.ts"
Task: "Add prohibited-terminology assertion in frontend/src/app/home/home-page/home-page.spec.ts"
```

## Parallel Example: User Story 2

```text
Task: "Add loading-state test in frontend/src/app/home/home-page/home-page.spec.ts"
Task: "Add empty-state test in frontend/src/app/home/home-page/home-page.spec.ts"
Task: "Add error-state test in frontend/src/app/home/home-page/home-page.spec.ts"
```

## Parallel Example: User Story 3

```text
Task: "Add details-link target tests in frontend/src/app/home/home-page/home-page.spec.ts"
Task: "Add keyboard/pointer accessible card semantics test in frontend/src/app/home/home-page/home-page.spec.ts"
```

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 setup.
2. Complete Phase 2 home shell.
3. Complete Phase 3 movie loading and card rendering.
4. Validate US1 independently at `/`.

### Incremental Delivery

1. US1: Home displays real movie cards.
2. US2: Add loading, empty, and error state resilience.
3. US3: Add details navigation and accessible activation.
4. Polish: Extract shared card only if it improves maintainability.

### Verification

1. Run `npm test -- --watch=false` from `frontend`.
2. Run `npm run build` from `frontend`.
3. Run backend checks only if implementation expands into backend/API files.
