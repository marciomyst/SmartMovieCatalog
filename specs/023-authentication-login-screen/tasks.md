# Tasks: Authentication Login Screen

**Input**: Design documents from `G:\new-work\SmartMovieCatalog\specs\023-authentication-login-screen`
**Prerequisites**: `plan.md`, `spec.md`, `research.md`, `data-model.md`, `contracts/auth-api.md`, `quickstart.md`
**Tests**: Included because `spec.md` FR-014 explicitly requires authentication test coverage.
**Organization**: Tasks are grouped by user story for independent implementation and verification.
**Scope**: Documentation and test alignment for already implemented auth behavior. Do not introduce refresh tokens, persistent sessions, logout, registration, password recovery, route guards, external identity providers, new backend behavior, or new dependencies.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files or is read-only verification.
- **[Story]**: Maps task to a user story from `spec.md`.
- Every task includes an exact file path.

## Phase 1: Setup (Shared Baseline)

**Purpose**: Establish the current implementation and documentation baseline before story-specific work.

- [X] T001 Review documentation-only scope in `specs/023-authentication-login-screen/spec.md`, `specs/023-authentication-login-screen/plan.md`, and `specs/023-authentication-login-screen/research.md`
- [X] T002 [P] Inventory existing frontend auth implementation under `frontend/src/app/auth`
- [X] T003 [P] Inventory existing backend auth contracts under `backend/src/SmartMovieCatalog.Contracts/Auth`
- [X] T004 [P] Inventory existing auth documentation in `docs/API.md`, `docs/FRONTEND.md`, `docs/SECURITY.md`, `docs/TESTING.md`, and `docs/adr/0008-frontend-auth-session.md`
- [X] T005 [P] Verify repository ignore hygiene for generated Node/.NET output in `.gitignore`, `.dockerignore`, and `.prettierignore`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Confirm cross-boundary auth facts shared by every user story.

**CRITICAL**: Complete this phase before story-specific tasks.

- [X] T006 Confirm `POST /api/auth/authenticate` and `GET /api/auth/me` are the only auth API dependencies in `specs/023-authentication-login-screen/contracts/auth-api.md`
- [X] T007 Confirm TypeScript auth models in `frontend/src/app/auth/auth.models.ts` align with C# contracts in `backend/src/SmartMovieCatalog.Contracts/Auth`
- [X] T008 Confirm raw auth `HttpClient` usage is isolated to `frontend/src/app/auth/auth-api.ts`
- [X] T009 Confirm login UI component code in `frontend/src/app/auth/login-page/login-page.ts` does not call `HttpClient` directly
- [X] T010 Confirm frontend session state in `frontend/src/app/auth/auth-session-store.ts` is memory-only and does not use browser storage

**Checkpoint**: API, model, HTTP-boundary, and session-boundary facts are verified.

---

## Phase 3: User Story 1 - Sign In (Priority: P1) MVP

**Goal**: Users can enter valid credentials, authenticate, load current-user context, and establish a frontend session.

**Independent Test**: Submit valid credentials and verify authenticate is called, current-user lookup is called with the bearer token, and the session store receives the authenticated session.

### Tests for User Story 1

- [X] T011 [P] [US1] Verify authenticate endpoint request/response coverage in `frontend/src/app/auth/auth-api.spec.ts`
- [X] T012 [P] [US1] Verify current-user request uses `Authorization: Bearer <token>` in `frontend/src/app/auth/auth-api.spec.ts`
- [X] T013 [P] [US1] Verify successful login calls authenticate, current-user lookup, and session storage in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T014 [P] [US1] Verify login form renders as the SPA entry surface in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 1

- [X] T015 [US1] Align sign-in acceptance scenarios in `specs/023-authentication-login-screen/spec.md`
- [X] T016 [US1] Align authenticate and current-user contract details in `specs/023-authentication-login-screen/contracts/auth-api.md`
- [X] T017 [US1] Align auth endpoint documentation in `docs/API.md`
- [X] T018 [US1] Align frontend login-flow documentation in `docs/FRONTEND.md`

**Checkpoint**: User Story 1 is independently testable and documented.

---

## Phase 4: User Story 2 - Validate Input (Priority: P1)

**Goal**: Users receive immediate field-level feedback for invalid email/password input and generic feedback for unauthorized authentication failures.

**Independent Test**: Submit empty or malformed input and verify field-level validation appears without authenticating; simulate backend validation and unauthorized responses and verify the displayed feedback.

### Tests for User Story 2

- [X] T019 [P] [US2] Verify required email and password validation messages in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T020 [P] [US2] Verify malformed email blocks authentication in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T021 [P] [US2] Verify password visibility toggle behavior in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T022 [P] [US2] Verify `400 ValidationProblemDetails` maps to field-level messages in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T023 [P] [US2] Verify `401 Unauthorized` shows generic authentication failure in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 2

- [X] T024 [US2] Align input validation requirements and edge cases in `specs/023-authentication-login-screen/spec.md`
- [X] T025 [US2] Align validation shapes and failure behavior in `specs/023-authentication-login-screen/data-model.md`
- [X] T026 [US2] Align `400 ValidationProblemDetails` and generic `401 ProblemDetails` frontend behavior in `docs/API.md`
- [X] T027 [US2] Align generic authentication failure security boundary in `docs/SECURITY.md`

**Checkpoint**: User Story 2 is independently testable and documented.

---

## Phase 5: User Story 3 - Preserve Session Boundaries (Priority: P2)

**Goal**: The authenticated frontend session remains in memory only and bearer tokens are not persisted in browser storage.

**Independent Test**: Complete sign-in, verify session state exists in memory, refresh the browser or recreate runtime state, and verify no token exists in `localStorage` or `sessionStorage`.

### Tests for User Story 3

- [X] T028 [P] [US3] Verify session store retains authenticated session in memory in `frontend/src/app/auth/auth-session-store.spec.ts`
- [X] T029 [P] [US3] Verify session store does not write bearer tokens to `localStorage` or `sessionStorage` in `frontend/src/app/auth/auth-session-store.spec.ts`
- [X] T030 [P] [US3] Verify failed current-user lookup cannot create a trusted session in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 3

- [X] T031 [US3] Align in-memory-only session rules and state transitions in `specs/023-authentication-login-screen/data-model.md`
- [X] T032 [US3] Align session-boundary acceptance criteria in `specs/023-authentication-login-screen/spec.md`
- [X] T033 [US3] Align frontend session model documentation in `docs/FRONTEND.md`
- [X] T034 [US3] Align bearer-token browser-storage rules in `docs/SECURITY.md`
- [X] T035 [US3] Align ADR consequences in `docs/adr/0008-frontend-auth-session.md`

**Checkpoint**: User Story 3 is independently testable and documented.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency checks and repository verification.

- [X] T036 [P] Run frontend auth tests with `npm test -- --watch=false` from `frontend`
- [X] T037 [P] Run frontend production build with `npm run build` from `frontend`
- [X] T038 Validate quickstart scenarios in `specs/023-authentication-login-screen/quickstart.md`
- [X] T039 Review final auth documentation for unsupported scope or secret material in `docs/API.md`, `docs/FRONTEND.md`, `docs/SECURITY.md`, `docs/TESTING.md`, and `docs/adr/0008-frontend-auth-session.md`
- [X] T040 Review `specs/023-authentication-login-screen/tasks.md` for strict checklist format and user-story traceability

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories.
- **User Stories (Phases 3-5)**: Depend on Foundational completion.
- **Polish (Phase 6)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational and is the MVP scope.
- **User Story 2 (P1)**: Can start after Foundational and can proceed in parallel with US1 because it focuses on validation and error feedback.
- **User Story 3 (P2)**: Can start after Foundational, but final wording should be reviewed after US1 to keep session behavior aligned with the sign-in flow.

### Within Each User Story

- Complete test tasks before documentation alignment tasks.
- Keep component/server-boundary changes in `frontend/src/app/auth`.
- Keep API contract documentation aligned with `backend/src/SmartMovieCatalog.Contracts/Auth`.
- Do not introduce runtime scope outside the spec non-goals.

---

## Parallel Opportunities

- T002, T003, T004, and T005 can run in parallel.
- T011, T012, T013, and T014 can run in parallel for User Story 1.
- T019, T020, T021, T022, and T023 can run in parallel for User Story 2.
- T028 and T029 can run in parallel for User Story 3.
- T036 and T037 can run in parallel after story tasks are complete.

---

## Parallel Example: User Story 1

```text
Task: "Verify authenticate endpoint request/response coverage in frontend/src/app/auth/auth-api.spec.ts"
Task: "Verify current-user request uses Authorization: Bearer <token> in frontend/src/app/auth/auth-api.spec.ts"
Task: "Verify successful login calls authenticate, current-user lookup, and session storage in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify login form renders as the SPA entry surface in frontend/src/app/auth/login-page/login-page.spec.ts"
```

---

## Parallel Example: User Story 2

```text
Task: "Verify required email and password validation messages in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify malformed email blocks authentication in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify password visibility toggle behavior in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify 400 ValidationProblemDetails maps to field-level messages in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify 401 Unauthorized shows generic authentication failure in frontend/src/app/auth/login-page/login-page.spec.ts"
```

---

## Parallel Example: User Story 3

```text
Task: "Verify session store retains authenticated session in memory in frontend/src/app/auth/auth-session-store.spec.ts"
Task: "Verify session store does not write bearer tokens to localStorage or sessionStorage in frontend/src/app/auth/auth-session-store.spec.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate the documented sign-in flow against the existing frontend auth tests and API docs.

### Incremental Delivery

1. Complete Setup and Foundational verification.
2. Deliver User Story 1 documentation and test alignment for sign-in.
3. Deliver User Story 2 validation and error-behavior coverage.
4. Deliver User Story 3 session-boundary coverage.
5. Run Polish verification and quickstart checks.

### Parallel Team Strategy

1. Complete Setup and Foundational tasks first.
2. Assign User Story 1 to auth API/login-flow coverage.
3. Assign User Story 2 to validation/error coverage.
4. Assign User Story 3 to in-memory session/security coverage.
5. Merge through Phase 6 verification.

---

## Notes

- [P] tasks touch different files or are read-only verification.
- [Story] labels map to user stories in `spec.md`.
- Tests are included because FR-014 explicitly requires them.
- Avoid runtime expansion beyond documented current behavior.
