# Tasks: Backend Authentication With JWT And Current User Context

**Input**: Design documents from `specs/021-backend-authentication-with-jwt-and-current-user-context/`
**Prerequisites**: `plan.md`, `spec.md`
**Optional artifacts found**: `research.md`, `data-model.md`, `quickstart.md`, `contracts/auth.openapi.yaml`
**Tests**: Required by the feature specification and acceptance criteria.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel because it touches different files and has no dependency on another incomplete task.
- **[Story]**: User story label, required only in user story phases.
- Every task below includes an exact repository path or generated migration directory.

## User Story Mapping

- **US1**: Valid credentials return a JWT access token and expiry.
- **US2**: Invalid credentials, nonexistent users, inactive users, and removed users receive generic authentication failures.
- **US3**: Authenticated clients can retrieve current user context.
- **US4**: Unauthenticated clients receive `401 Unauthorized` for current-user lookup.
- **US5**: Tokens for removed, missing, or inactive users are rejected during current-user lookup.

---

## Phase 1: Setup

**Purpose**: Add deliberate package references and test scaffolding authorized by ADR 0002, ADR 0003, and ADR 0004.

- [X] T001 Add JWT bearer and EF Core design-time package references in `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.csproj`
- [X] T002 Add EF Core, Npgsql provider, and password hashing package references in `backend/src/SmartMovieCatalog.Infrastructure/SmartMovieCatalog.Infrastructure.csproj`
- [X] T003 Add application test framework, assertions, and project references in `backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj`
- [X] T004 Add API test framework, assertions, `Microsoft.AspNetCore.Mvc.Testing`, and project references in `backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj`
- [X] T005 [P] Add infrastructure test framework references in `backend/tests/SmartMovieCatalog.Infrastructure.Tests/SmartMovieCatalog.Infrastructure.Tests.csproj`
- [X] T006 [P] Update backend test tooling notes for the selected packages in `docs/TESTING.md`

---

## Phase 2: Foundational

**Purpose**: Shared contracts, domain model, application abstractions, persistence, authentication middleware, and test helpers that block all user stories.

**Checkpoint**: No user story implementation should begin until this phase is complete.

- [X] T007 [P] Add public auth DTOs in `backend/src/SmartMovieCatalog.Contracts/Auth/AuthenticateRequest.cs`, `backend/src/SmartMovieCatalog.Contracts/Auth/AuthenticateResponse.cs`, and `backend/src/SmartMovieCatalog.Contracts/Auth/CurrentUserResponse.cs`
- [X] T008 [P] Add the framework-free user aggregate and user id value object in `backend/src/SmartMovieCatalog.Domain/Users/User.cs` and `backend/src/SmartMovieCatalog.Domain/Users/UserId.cs`
- [X] T009 [P] Add email and role value objects in `backend/src/SmartMovieCatalog.Domain/Users/EmailAddress.cs` and `backend/src/SmartMovieCatalog.Domain/Users/UserRole.cs`
- [X] T010 [P] Add authentication result models in `backend/src/SmartMovieCatalog.Application/Features/Auth/AuthenticationResult.cs`, `backend/src/SmartMovieCatalog.Application/Features/Auth/CurrentUserResult.cs`, and `backend/src/SmartMovieCatalog.Application/Features/Auth/AuthenticationFailure.cs`
- [X] T011 Add application authentication abstractions in `backend/src/SmartMovieCatalog.Application/Abstractions/Authentication/IPasswordHasher.cs`, `backend/src/SmartMovieCatalog.Application/Abstractions/Authentication/IAccessTokenService.cs`, and `backend/src/SmartMovieCatalog.Application/Abstractions/Authentication/ICurrentUserPrincipalAccessor.cs`
- [X] T012 Add persistence and time abstractions in `backend/src/SmartMovieCatalog.Application/Abstractions/Persistence/IUserRepository.cs` and `backend/src/SmartMovieCatalog.Application/Abstractions/Time/IClock.cs`
- [X] T013 Add JWT and database option models with validation in `backend/src/SmartMovieCatalog.Infrastructure/Authentication/JwtOptions.cs` and `backend/src/SmartMovieCatalog.Infrastructure/Persistence/DatabaseOptions.cs`
- [X] T014 Add EF Core DbContext and design-time factory in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/SmartMovieCatalogDbContext.cs` and `backend/src/SmartMovieCatalog.Infrastructure/Persistence/SmartMovieCatalogDbContextFactory.cs`
- [X] T015 Add user persistence mapping in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Configurations/UserConfiguration.cs`
- [X] T016 Add the initial user and role EF Core migration in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/Migrations/`
- [X] T017 Add development/test-only user seeding support in `backend/src/SmartMovieCatalog.Infrastructure/Persistence/DevelopmentUserSeeder.cs`
- [X] T018 Wire EF Core, repository, password hasher, JWT service, current-user accessor, and clock registrations in `backend/src/SmartMovieCatalog.Infrastructure/DependencyInjection.cs`
- [X] T019 Register application auth use cases in `backend/src/SmartMovieCatalog.Application/DependencyInjection.cs`
- [X] T020 Configure JWT authentication, authorization, option validation, `UseAuthentication()`, and middleware ordering in `backend/src/SmartMovieCatalog.Api/Program.cs`
- [X] T021 Add product auth error helpers using `ProblemDetails` and `ValidationProblemDetails` in `backend/src/SmartMovieCatalog.Api/Common/AuthProblemDetails.cs`
- [X] T022 Add reusable application test doubles in `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeUserRepository.cs`, `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakePasswordHasher.cs`, `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeAccessTokenService.cs`, and `backend/tests/SmartMovieCatalog.Application.Tests/TestSupport/FakeClock.cs`
- [X] T023 Add reusable API test host and auth fixtures in `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/SmartMovieCatalogApiFactory.cs`, `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/AuthApiFixture.cs`, and `backend/tests/SmartMovieCatalog.Api.Tests/TestSupport/TestJwtOptions.cs`

---

## Phase 3: User Story 1 - Authenticate With Valid Credentials (Priority: P1)

**Goal**: An API client can submit valid email/password credentials and receive a signed JWT access token with an expiry timestamp.

**Independent Test**: Arrange an active, non-removed user; call `POST /api/auth/authenticate`; verify `200 OK` with `userId`, `email`, `accessToken`, and `accessTokenExpiresAtUtc`.

### Tests For User Story 1

- [X] T024 [P] [US1] Add application tests for successful authentication in `backend/tests/SmartMovieCatalog.Application.Tests/Auth/AuthenticateUserTests.cs`
- [X] T025 [P] [US1] Add API tests for successful `POST /api/auth/authenticate` in `backend/tests/SmartMovieCatalog.Api.Tests/Auth/AuthenticateEndpointTests.cs`

### Implementation For User Story 1

- [X] T026 [US1] Implement the authentication use case in `backend/src/SmartMovieCatalog.Application/Features/Auth/AuthenticateUser.cs`
- [X] T027 [US1] Implement normalized-email user lookup and active-user persistence behavior in `backend/src/SmartMovieCatalog.Infrastructure/Authentication/EfUserRepository.cs`
- [X] T028 [US1] Implement secure framework-backed password hashing and verification in `backend/src/SmartMovieCatalog.Infrastructure/Authentication/PasswordHasher.cs`
- [X] T029 [US1] Implement JWT access token creation with subject and role claims in `backend/src/SmartMovieCatalog.Infrastructure/Authentication/JwtAccessTokenService.cs`
- [X] T030 [US1] Add `POST /api/auth/authenticate` endpoint handling in `backend/src/SmartMovieCatalog.Api/Controllers/AuthController.cs`
- [X] T031 [US1] Update the login request example in `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.http`

**Checkpoint**: US1 is complete when valid credentials return a token and all US1 tests pass independently.

---

## Phase 4: User Story 2 - Reject Invalid Authentication Attempts (Priority: P1)

**Goal**: Invalid input and failed credentials return the correct HTTP status without disclosing whether the email exists.

**Independent Test**: Call `POST /api/auth/authenticate` with malformed input, wrong password, nonexistent email, inactive user, and removed user; verify `400 Bad Request` for validation failures and generic `401 Unauthorized` for authentication failures.

### Tests For User Story 2

- [X] T032 [P] [US2] Add application tests for invalid password, nonexistent user, inactive user, and removed user in `backend/tests/SmartMovieCatalog.Application.Tests/Auth/AuthenticateUserFailureTests.cs`
- [X] T033 [P] [US2] Add API tests for `400 Bad Request` and generic `401 Unauthorized` authentication failures in `backend/tests/SmartMovieCatalog.Api.Tests/Auth/AuthenticateEndpointFailureTests.cs`

### Implementation For User Story 2

- [X] T034 [US2] Add authentication request validation without echoing passwords in `backend/src/SmartMovieCatalog.Api/Controllers/AuthController.cs`
- [X] T035 [US2] Map authentication failures to generic `ProblemDetails` in `backend/src/SmartMovieCatalog.Api/Common/AuthProblemDetails.cs`
- [X] T036 [US2] Reject inactive and removed users in `backend/src/SmartMovieCatalog.Application/Features/Auth/AuthenticateUser.cs`
- [X] T037 [US2] Add safe authentication failure logging without passwords, hashes, tokens, or enumeration details in `backend/src/SmartMovieCatalog.Api/Controllers/AuthController.cs`

**Checkpoint**: US2 is complete when invalid auth attempts fail with the accepted error contract and no account-enumeration detail.

---

## Phase 5: User Story 3 - Retrieve Current User Context (Priority: P2)

**Goal**: An authenticated client can retrieve current user identity, roles, and first-login password-change state.

**Independent Test**: Authenticate or mint a valid token for an active user; call `GET /api/auth/me`; verify `200 OK` with `userId`, `email`, `name`, `roles`, and `mustChangePasswordOnFirstLogin`.

### Tests For User Story 3

- [X] T038 [P] [US3] Add application tests for successful current-user lookup in `backend/tests/SmartMovieCatalog.Application.Tests/Auth/GetCurrentUserTests.cs`
- [X] T039 [P] [US3] Add API tests for successful `GET /api/auth/me` with a valid token in `backend/tests/SmartMovieCatalog.Api.Tests/Auth/CurrentUserEndpointTests.cs`

### Implementation For User Story 3

- [X] T040 [US3] Implement current principal extraction from bearer claims in `backend/src/SmartMovieCatalog.Infrastructure/Authentication/HttpCurrentUserPrincipalAccessor.cs`
- [X] T041 [US3] Implement the current-user use case in `backend/src/SmartMovieCatalog.Application/Features/Auth/GetCurrentUser.cs`
- [X] T042 [US3] Add authorized `GET /api/auth/me` endpoint handling in `backend/src/SmartMovieCatalog.Api/Controllers/AuthController.cs`
- [X] T043 [US3] Update the current-user request example in `backend/src/SmartMovieCatalog.Api/SmartMovieCatalog.Api.http`

**Checkpoint**: US3 is complete when authenticated current-user lookup returns the expected identity contract.

---

## Phase 6: User Story 4 - Reject Unauthenticated Current-User Requests (Priority: P2)

**Goal**: Requests without a valid bearer token receive `401 Unauthorized`.

**Independent Test**: Call `GET /api/auth/me` without an authorization header and with malformed, expired, or incorrectly signed tokens; verify `401 Unauthorized`.

### Tests For User Story 4

- [X] T044 [P] [US4] Add API tests for missing, malformed, expired, and incorrectly signed bearer tokens in `backend/tests/SmartMovieCatalog.Api.Tests/Auth/CurrentUserUnauthorizedTests.cs`

### Implementation For User Story 4

- [X] T045 [US4] Ensure `GET /api/auth/me` requires authorization metadata in `backend/src/SmartMovieCatalog.Api/Controllers/AuthController.cs`
- [X] T046 [US4] Configure JWT bearer challenge behavior for accepted `ProblemDetails` responses in `backend/src/SmartMovieCatalog.Api/Program.cs`

**Checkpoint**: US4 is complete when unauthenticated current-user requests consistently return `401 Unauthorized`.

---

## Phase 7: User Story 5 - Reject Stale User Tokens (Priority: P3)

**Goal**: A token is rejected when its subject no longer maps to an existing active, non-removed user.

**Independent Test**: Mint or reuse a token for a user that is later deactivated, removed, or deleted; call `GET /api/auth/me`; verify `401 Unauthorized`.

### Tests For User Story 5

- [X] T047 [P] [US5] Add application tests for missing, inactive, and removed persisted users in `backend/tests/SmartMovieCatalog.Application.Tests/Auth/GetCurrentUserFailureTests.cs`
- [X] T048 [P] [US5] Add API tests for stale-token current-user rejection in `backend/tests/SmartMovieCatalog.Api.Tests/Auth/CurrentUserStaleTokenTests.cs`

### Implementation For User Story 5

- [X] T049 [US5] Reject missing, inactive, and removed persisted users in `backend/src/SmartMovieCatalog.Application/Features/Auth/GetCurrentUser.cs`
- [X] T050 [US5] Map stale-token current-user failures to `401 Unauthorized` in `backend/src/SmartMovieCatalog.Api/Common/AuthProblemDetails.cs`

**Checkpoint**: US5 is complete when current-user lookup validates persisted user state on every request.

---

## Phase 8: Polish And Cross-Cutting Concerns

**Purpose**: Documentation, examples, scaffold cleanup, and final verification across all stories.

- [X] T051 [P] Update backend API documentation for auth endpoints and accepted error responses in `docs/API.md`
- [X] T052 [P] Update security documentation for password hashing, JWT configuration, secret handling, user provisioning, and auth logging in `docs/SECURITY.md`
- [X] T053 [P] Update local setup and user provisioning notes in `README.md`
- [X] T054 [P] Add safe JWT and database environment variable placeholders without secret values in `docker-compose.yml`
- [X] T055 Remove WeatherForecast scaffold endpoint and model in `backend/src/SmartMovieCatalog.Api/Controllers/WeatherForecastController.cs` and `backend/src/SmartMovieCatalog.Api/WeatherForecast.cs`
- [X] T056 Run backend auth tests for `backend/tests/SmartMovieCatalog.Application.Tests/SmartMovieCatalog.Application.Tests.csproj` and `backend/tests/SmartMovieCatalog.Api.Tests/SmartMovieCatalog.Api.Tests.csproj`
- [X] T057 Run final solution verification for `SmartMovieCatalog.slnx`

---

## Dependencies And Execution Order

### Phase Dependencies

- **Phase 1 Setup**: Must complete first because package and test framework choices unblock implementation.
- **Phase 2 Foundational**: Depends on Phase 1 and blocks every user story.
- **US1 and US2**: Can start after Phase 2. US2 hardens the login path and should ship with US1.
- **US3 and US4**: Can start after Phase 2. US4 hardens the current-user endpoint and should ship with US3.
- **US5**: Depends on US3 current-user lookup.
- **Phase 8 Polish**: Depends on the selected user stories being complete.

### User Story Dependencies

- **US1 (P1)**: MVP token issuance after foundation.
- **US2 (P1)**: Login failure handling after the login use case and endpoint exist.
- **US3 (P2)**: Current-user lookup after authentication foundation.
- **US4 (P2)**: Bearer enforcement after current-user endpoint exists.
- **US5 (P3)**: Stale-token rejection after current-user lookup exists.

### Within Each Story

- Write the listed tests before implementation tasks in that story.
- Keep domain and application behavior outside controllers.
- Implement application services before endpoint wiring.
- Keep authentication failures generic and avoid logging secrets or enumeration details.

---

## Parallel Opportunities

- T003, T004, T005, and T006 can run in parallel after T001 and T002 are understood.
- T007, T008, T009, and T010 can run in parallel after Phase 1.
- T024 and T025 can run in parallel for US1 tests.
- T032 and T033 can run in parallel for US2 failure coverage.
- T038 and T039 can run in parallel for US3 current-user coverage.
- T047 and T048 can run in parallel for US5 stale-token coverage.
- T051, T052, T053, and T054 can run in parallel during polish.

## Parallel Example: US1

```text
Task: "T024 [P] [US1] Add application tests for successful authentication in backend/tests/SmartMovieCatalog.Application.Tests/Auth/AuthenticateUserTests.cs"
Task: "T025 [P] [US1] Add API tests for successful POST /api/auth/authenticate in backend/tests/SmartMovieCatalog.Api.Tests/Auth/AuthenticateEndpointTests.cs"
```

## Parallel Example: Foundational Domain And Contracts

```text
Task: "T007 [P] Add public auth DTOs in backend/src/SmartMovieCatalog.Contracts/Auth/AuthenticateRequest.cs, backend/src/SmartMovieCatalog.Contracts/Auth/AuthenticateResponse.cs, and backend/src/SmartMovieCatalog.Contracts/Auth/CurrentUserResponse.cs"
Task: "T008 [P] Add the framework-free user aggregate and user id value object in backend/src/SmartMovieCatalog.Domain/Users/User.cs and backend/src/SmartMovieCatalog.Domain/Users/UserId.cs"
Task: "T009 [P] Add email and role value objects in backend/src/SmartMovieCatalog.Domain/Users/EmailAddress.cs and backend/src/SmartMovieCatalog.Domain/Users/UserRole.cs"
```

---

## Implementation Strategy

### MVP First

1. Complete Phase 1 and Phase 2.
2. Complete US1 and US2 together so login success and login failure behavior ship as one secure increment.
3. Validate with application and API tests for `POST /api/auth/authenticate`.

### Incremental Delivery

1. Add US1 plus US2 for authentication.
2. Add US3 plus US4 for current-user lookup and bearer-token enforcement.
3. Add US5 for stale-token rejection against persisted user state.
4. Complete documentation, HTTP examples, Docker environment placeholders, scaffold cleanup, and final verification.

### Final Verification

1. Run backend application and API tests for the added test projects.
2. Run `dotnet build SmartMovieCatalog.slnx`.