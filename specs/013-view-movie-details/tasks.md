# Tasks: View Movie Details

**Input**: Design documents from `/specs/013-view-movie-details/`
**Prerequisites**: plan.md (required), spec.md (required for user stories)

**Tests**: Test tasks were not included because the feature spec does not explicitly require a TDD or test-first workflow.

**Organization**: Tasks are grouped by user story to enable independent implementation and validation of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Prepare the feature slice structure and shared contract surface.

- [ ] T001 Create the details contract file `backend/src/SmartMovieCatalog.Contracts/Movies/MovieDetailsResponse.cs`
- [ ] T002 Create application get-by-id query/handler files under `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [ ] T003 [P] Create API endpoint slice folder `backend/src/SmartMovieCatalog.Api/Features/Movies/GetMovieById/`
- [ ] T004 [P] Create frontend details page folder `frontend/src/app/movies/movie-details-page/`
---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core backend/frontend foundations required before user story implementation.

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [ ] T005 Extend repository abstraction with single-movie lookup in `backend/src/SmartMovieCatalog.Application/Abstractions/Persistence/IMovieRepository.cs`
- [ ] T006 Implement single-movie lookup with genres include in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/EfMovieRepository.cs`
- [ ] T007 [P] Update application fake repository for new lookup contract in `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeMovieRepository.cs`
- [ ] T008 [P] Update API fake repository for new lookup contract in `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/FakeMovieRepository.cs`
- [ ] T009 Add frontend details models in `frontend/src/app/movies/movie.models.ts`
- [ ] T010 Add typed details API method in `frontend/src/app/movies/movies-api.ts`

**Checkpoint**: Foundation ready - user story implementation can now begin.

---

## Phase 3: User Story 1 - Open Movie Details From Catalog (Priority: P1) 🎯 MVP

**Goal**: Allow users to open `/movies/{id}` and view full movie metadata.

**Independent Test**: Open `/movies/{id}` for an existing movie and confirm title, year, genres, director, synopsis, duration, age rating, and available metadata render correctly.

### Implementation for User Story 1

- [ ] T011 [US1] Implement application details projection model in `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [ ] T012 [US1] Implement get-by-id query handler orchestration in `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [ ] T013 [US1] Implement `GET /api/movies/{id}` endpoint response mapping in `backend/src/SmartMovieCatalog.Api/Features/Movies/GetMovieById/GetMovieByIdEndpoint.cs`
- [ ] T014 [US1] Register details endpoint mapping in `backend/src/SmartMovieCatalog.Api/Features/Movies/MoviesEndpoints.cs`
- [ ] T015 [US1] Add details page component logic in `frontend/src/app/movies/movie-details-page/movie-details-page.ts`
- [ ] T016 [P] [US1] Add details page template in `frontend/src/app/movies/movie-details-page/movie-details-page.html`
- [ ] T017 [P] [US1] Add details page styling in `frontend/src/app/movies/movie-details-page/movie-details-page.css`
- [ ] T018 [US1] Register `/movies/:id` route in `frontend/src/main.ts`
- [ ] T019 [US1] Confirm and adjust catalog navigation links to details in `frontend/src/app/catalog/catalog-page/catalog-page.ts` and `frontend/src/app/catalog/catalog-page/catalog-page.html`

**Checkpoint**: User Story 1 is functional and can be validated independently.

---

## Phase 4: User Story 2 - Show Not Found State (Priority: P2)

**Goal**: Return and render a clear not-found state when the movie does not exist.

**Independent Test**: Open `/movies/{id}` with a valid GUID that does not exist and confirm API returns `404 problem+json` and UI shows a not-found state.

### Implementation for User Story 2

- [ ] T020 [US2] Add application not-found result type in `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [ ] T021 [US2] Update get-by-id handler to return success/failure result in `backend/src/SmartMovieCatalog.Application/Features/Movies/`
- [ ] T022 [US2] Map not-found result to RFC7807 response in `backend/src/SmartMovieCatalog.Api/Features/Movies/GetMovieById/GetMovieByIdEndpoint.cs`
- [ ] T023 [US2] Add details page not-found state rendering in `frontend/src/app/movies/movie-details-page/movie-details-page.ts` and `frontend/src/app/movies/movie-details-page/movie-details-page.html`

**Checkpoint**: User Story 2 is functional and can be validated independently.

---

## Phase 5: User Story 3 - Show API Failure State (Priority: P3)

**Goal**: Show a clear recoverable error state for request failures other than not-found.

**Independent Test**: Simulate API failure while loading `/movies/{id}` and confirm UI renders error state without crashing.

### Implementation for User Story 3

- [ ] T024 [US3] Add details load-state model for generic failure handling in `frontend/src/app/movies/movie.models.ts`
- [ ] T025 [US3] Implement HTTP error classification (404 vs other failures) in `frontend/src/app/movies/movie-details-page/movie-details-page.ts`
- [ ] T026 [US3] Add failure UI state in `frontend/src/app/movies/movie-details-page/movie-details-page.html` and `frontend/src/app/movies/movie-details-page/movie-details-page.css`

**Checkpoint**: User Story 3 is functional and can be validated independently.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency updates across docs and verification.

- [ ] T027 [P] Document new endpoint and response contract in `docs/API.md`
- [ ] T028 [P] Document frontend route/details behavior in `docs/FRONTEND.md`
- [ ] T029 Verify backend build and tests from `SmartMovieCatalog.slnx`
- [ ] T030 Verify frontend build and tests from `frontend/package.json`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - start immediately
- **Foundational (Phase 2)**: Depends on Setup - blocks all user stories
- **User Stories (Phases 3-5)**: Depend on Foundational completion
  - US1 (P1) first for MVP
  - US2 (P2) depends on US1 endpoint/page baseline
  - US3 (P3) depends on US1 page load flow baseline
- **Polish (Phase 6)**: Depends on completion of selected user stories

### User Story Dependencies

- **US1**: Starts after Foundational; no dependency on US2/US3
- **US2**: Starts after US1 backend/frontend details baseline
- **US3**: Starts after US1 frontend load flow baseline

### Within Each User Story

- Backend application/repository wiring before API endpoint mapping
- API response behavior before frontend state rendering
- Route/navigation wiring after details component is implemented

### Parallel Opportunities

- Phase 1: T003 and T004 can run in parallel
- Phase 2: T007 and T008 can run in parallel; T009 can run in parallel with backend tasks
- US1: T016 and T017 can run in parallel after T015 starts
- Polish: T027 and T028 can run in parallel

---

## Parallel Example: User Story 1

```bash
Task: "T016 [US1] Add details page template in frontend/src/app/movies/movie-details-page/movie-details-page.html"
Task: "T017 [US1] Add details page styling in frontend/src/app/movies/movie-details-page/movie-details-page.css"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1 and Phase 2
2. Complete Phase 3 (US1)
3. Validate existing movie details end-to-end (`GET /api/movies/{id}` + `/movies/{id}`)
4. Demo/deploy MVP increment

### Incremental Delivery

1. Deliver US1 (details retrieval + details page)
2. Deliver US2 (not-found behavior with RFC7807 response + UI state)
3. Deliver US3 (generic API error state)
4. Finish docs and verification in Phase 6

### Parallel Team Strategy

1. One developer completes backend foundations (T005-T008) while another prepares frontend foundations (T009-T010)
2. After foundations, backend/API and frontend details UI can progress in parallel for US1
3. US2 backend mapping and US3 frontend error UX can proceed in parallel after US1 baseline

---

## Notes

- [P] tasks are file-disjoint and can be executed in parallel safely.
- All user story tasks include `[US#]` labels for traceability.
- Tasks are intentionally scoped to existing architecture and supported metadata only.
