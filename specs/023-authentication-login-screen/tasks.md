# Tasks: Authentication Login Screen

**Input**: Design documents from `G:\new-work\SmartMovieCatalog\specs\023-authentication-login-screen`
**Prerequisites**: `plan.md`, `spec.md`
**Available Docs**: No optional `research.md`, `data-model.md`, `contracts/`, or `quickstart.md` artifacts are present.
**Tests**: Test-related tasks are included because `spec.md` explicitly requires auth test coverage in FR-014.
**Scope**: Documentation/spec-kit alignment for already implemented auth behavior. No runtime code changes are planned unless a task uncovers drift.

## Phase 1: Setup (Shared Documentation Baseline)

**Purpose**: Confirm the feature remains a documentation-only slice tied to the existing frontend auth implementation.

- [X] T001 Review documentation-only scope in `specs/023-authentication-login-screen/spec.md` and `specs/023-authentication-login-screen/plan.md`
- [X] T002 [P] Inventory frontend auth implementation files under `frontend/src/app/auth`
- [X] T003 [P] Inventory auth API contract files under `backend/src/SmartMovieCatalog.Contracts/Auth`
- [X] T004 [P] Inventory auth documentation in `docs/API.md`, `docs/FRONTEND.md`, `docs/SECURITY.md`, and `docs/adr/0008-frontend-auth-session.md`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Establish the cross-boundary facts all user-story documentation depends on.

**CRITICAL**: No user story task should proceed until these facts are checked.

- [X] T005 Confirm `POST /api/auth/authenticate` and `GET /api/auth/me` are the only auth API dependencies documented in `docs/API.md`
- [X] T006 Confirm frontend auth contracts in `frontend/src/app/auth/auth.models.ts` align with `backend/src/SmartMovieCatalog.Contracts/Auth`
- [X] T007 Confirm `frontend/src/app/auth/auth-api.ts` owns auth `HttpClient` calls and UI components do not call `HttpClient` directly
- [X] T008 Confirm `frontend/src/app/auth/auth-session-store.ts` keeps the authenticated session in memory only

**Checkpoint**: Documentation facts are verified and user-story alignment can proceed.

---

## Phase 3: User Story 1 - Sign In (Priority: P1) MVP

**Goal**: Document that users can submit valid credentials, authenticate, and load the current user context.

**Independent Test**: Open the app, submit valid credentials, and verify the frontend calls authenticate, then current-user lookup, then stores the session.

### Tests for User Story 1

- [X] T009 [P] [US1] Verify authenticate endpoint coverage in `frontend/src/app/auth/auth-api.spec.ts`
- [X] T010 [P] [US1] Verify bearer header current-user coverage in `frontend/src/app/auth/auth-api.spec.ts`
- [X] T011 [P] [US1] Verify successful login submission and session storage coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 1

- [X] T012 [US1] Align sign-in user story and acceptance scenarios in `specs/023-authentication-login-screen/spec.md`
- [X] T013 [US1] Align auth endpoint request/response documentation in `docs/API.md`
- [X] T014 [US1] Align frontend login flow documentation in `docs/FRONTEND.md`

**Checkpoint**: User Story 1 is documented and independently verifiable.

---

## Phase 4: User Story 2 - Validate Input (Priority: P1)

**Goal**: Document immediate client-side validation and backend validation-error mapping for login input.

**Independent Test**: Submit empty or malformed input and verify field-level validation appears without authenticating.

### Tests for User Story 2

- [X] T015 [P] [US2] Verify required email and password validation coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T016 [P] [US2] Verify password visibility toggle coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T017 [P] [US2] Verify backend validation error mapping coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`
- [X] T018 [P] [US2] Verify generic unauthorized error coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 2

- [X] T019 [US2] Align input validation requirements and edge cases in `specs/023-authentication-login-screen/spec.md`
- [X] T020 [US2] Align `400 ValidationProblemDetails` and generic `401 ProblemDetails` frontend behavior in `docs/API.md`
- [X] T021 [US2] Align generic authentication failure security boundary in `docs/SECURITY.md`

**Checkpoint**: User Story 2 is documented and independently verifiable.

---

## Phase 5: User Story 3 - Preserve Session Boundaries (Priority: P2)

**Goal**: Document that the frontend does not persist bearer tokens and a browser refresh clears the session.

**Independent Test**: Complete sign in, refresh the browser, and verify no bearer token exists in `localStorage` or `sessionStorage`.

### Tests for User Story 3

- [X] T022 [P] [US3] Verify in-memory session behavior coverage in `frontend/src/app/auth/auth-session-store.ts` and related frontend auth tests
- [X] T023 [P] [US3] Verify no persistent browser storage assertions are needed or add focused coverage in `frontend/src/app/auth/login-page/login-page.spec.ts`

### Implementation for User Story 3

- [X] T024 [US3] Align in-memory-only session requirements and edge cases in `specs/023-authentication-login-screen/spec.md`
- [X] T025 [US3] Align frontend session model documentation in `docs/FRONTEND.md`
- [X] T026 [US3] Align bearer-token storage rules in `docs/SECURITY.md`
- [X] T027 [US3] Align ADR session consequences in `docs/adr/0008-frontend-auth-session.md`

**Checkpoint**: User Story 3 is documented and independently verifiable.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Final consistency and verification across the spec-kit artifact, docs, and existing frontend tests.

- [ ] T028 [P] Run frontend auth test suite with `npm test -- --watch=false` from `frontend`
- [ ] T029 [P] Run frontend production build with `npm run build` from `frontend`
- [ ] T030 Review `specs/023-authentication-login-screen/tasks.md` for strict checklist format and user-story traceability
- [ ] T031 Review final documentation for accidental secrets, token examples beyond placeholders, or unsupported scope in `specs/023-authentication-login-screen/spec.md`, `docs/API.md`, `docs/FRONTEND.md`, `docs/SECURITY.md`, and `docs/adr/0008-frontend-auth-session.md`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies.
- **Foundational (Phase 2)**: Depends on Setup completion and blocks all user stories.
- **User Stories (Phases 3-5)**: Depend on Foundational completion.
- **Polish (Phase 6)**: Depends on all desired user stories being complete.

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational and is the MVP scope.
- **User Story 2 (P1)**: Can start after Foundational and can be completed in parallel with User Story 1 because it owns validation/error documentation.
- **User Story 3 (P2)**: Can start after Foundational and can be completed independently, but should be reviewed after User Story 1 to keep session wording aligned with the sign-in flow.

### Within Each User Story

- Complete test verification tasks before documentation alignment tasks.
- Keep edits scoped to the listed files.
- Do not introduce runtime behavior changes unless verification uncovers drift from the accepted spec and plan.

---

## Parallel Opportunities

- T002, T003, and T004 can run in parallel.
- T009, T010, and T011 can run in parallel for User Story 1.
- T015, T016, T017, and T018 can run in parallel for User Story 2.
- T022 and T023 can run in parallel for User Story 3.
- T028 and T029 can run in parallel after documentation alignment is complete.

---

## Parallel Example: User Story 1

```text
Task: "Verify authenticate endpoint coverage in frontend/src/app/auth/auth-api.spec.ts"
Task: "Verify bearer header current-user coverage in frontend/src/app/auth/auth-api.spec.ts"
Task: "Verify successful login submission and session storage coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
```

---

## Parallel Example: User Story 2

```text
Task: "Verify required email and password validation coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify password visibility toggle coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify backend validation error mapping coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
Task: "Verify generic unauthorized error coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
```

---

## Parallel Example: User Story 3

```text
Task: "Verify in-memory session behavior coverage in frontend/src/app/auth/auth-session-store.ts and related frontend auth tests"
Task: "Verify no persistent browser storage assertions are needed or add focused coverage in frontend/src/app/auth/login-page/login-page.spec.ts"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup.
2. Complete Phase 2: Foundational.
3. Complete Phase 3: User Story 1.
4. Stop and validate the documented sign-in flow against the existing frontend auth tests and docs.

### Incremental Delivery

1. Complete Setup and Foundational verification.
2. Deliver User Story 1 documentation alignment for sign-in.
3. Deliver User Story 2 documentation alignment for validation and error behavior.
4. Deliver User Story 3 documentation alignment for in-memory session boundaries.
5. Run final frontend verification and documentation security review.

### Team Parallel Strategy

1. Complete Setup and Foundational tasks first.
2. Assign User Story 1 to auth API/login flow documentation.
3. Assign User Story 2 to validation and error documentation.
4. Assign User Story 3 to session security documentation.
5. Merge through the Polish phase checks.
